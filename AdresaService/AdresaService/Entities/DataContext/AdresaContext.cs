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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AdresaDB"));
        }


        /// <summary>
        /// Popunjava bazu sa nekim inicijalnim podacima
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Drzava>()
                .HasData(new
                {
                    DrzavaId = Guid.Parse("f320743f-6c87-47ca-9f82-50191c1d31ac"),
                    NazivDrzave = "Srbija"
                });

            builder.Entity<Drzava>()
               .HasData(new
               {
                   DrzavaId = Guid.Parse("9ce21ce2-7809-4e28-ba74-fd2f1bc6466a"),
                   NazivDrzave = "Bosna i hercegovina"
               });

        }

     }
}
