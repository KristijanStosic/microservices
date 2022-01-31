using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UgovorOZakupu.Entities;

namespace UgovorOZakupu.DbContext
{
    public class UgovorOZakupuDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IConfiguration _configuration;

        public UgovorOZakupuDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Entities.UgovorOZakupu> UgovoriOZakupu { get; set; }
        public DbSet<TipGarancije> TipoviGarancije { get; set; }
        public DbSet<RokDospeca> RokoviDospeca { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("UgovorOZakupuDB"));
        }
    }
}