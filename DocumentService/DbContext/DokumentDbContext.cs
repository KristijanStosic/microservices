using DocumentService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DocumentService.DbContext
{
    public class DokumentDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IConfiguration _configuration;

        public DokumentDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Dokument> Dokumenti { get; set; }
        public DbSet<TipDokumenta> TipoviDokumenta { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DokumentDB"));
        }
    }
}