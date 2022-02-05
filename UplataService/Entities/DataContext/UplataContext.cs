using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace UplataService.Entities.DataContext
{
    public class UplataContext : DbContext
    {
        private  IConfiguration _configuration;
        public UplataContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Uplata> Uplata { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("UplataDB"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Uplata>(b =>
            {
                b.ToTable("Uplata").HasKey(x => x.UplataId);
                b.Property(x => x.BrojRacuna).HasMaxLength(20).HasDefaultValue(string.Empty);
                b.Property(x => x.PozivNaBroj).HasMaxLength(255).HasDefaultValue(string.Empty);
                b.Property(x => x.SvrhaUplate).HasMaxLength(255).HasDefaultValue(string.Empty);
                b.Property(x => x.Iznos);
                b.Property(x => x.DatumUplate);

                b.OwnsOne(x => x.Kurs, sb =>
                {
                    sb.Property(x => x.VrednostKursa);
                });
            });

            builder.Entity<Uplata>()
                .HasData(new
                {
                    UplataId = Guid.Parse("8E297AD0-9072-4941-B951-5970EAED18F3"),
                    BrojRacuna = "100-4777487000005-66",
                    PozivNaBroj = "90-555258-552-559",
                    Iznos = 1999.99,
                    SvrhaUplate = "Uplata na racun",
                    DatumUplate = DateTime.Parse("2020-05-25")
                },
                new
                {
                    UplataId = Guid.Parse("19D28646-6779-4896-A9FF-6E7B7B70D87A"),
                    BrojRacuna = "150-2541485965214-99",
                    PozivNaBroj = "90-555258-552-559",
                    Iznos = 2052.47,
                    SvrhaUplate = "Uplata na racun",
                    DatumUplate = DateTime.Parse("2020-05-25")
                },
                new
                {
                    UplataId = Guid.Parse("633140E2-3FBC-4402-BD4D-EC6E06EC6627"),
                    BrojRacuna = "150-3333385965214-99",
                    PozivNaBroj = "90-555258-552-559",
                    Iznos = 8482.99,
                    SvrhaUplate = "Uplata na racun",
                    DatumUplate = DateTime.Parse("2020-05-25")
                });


        }
    }
}
