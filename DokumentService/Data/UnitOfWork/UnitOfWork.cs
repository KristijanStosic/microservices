using System.Threading.Tasks;
using DokumentService.DbContext;

namespace DokumentService.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DokumentDbContext _context;

        public UnitOfWork(DokumentDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}