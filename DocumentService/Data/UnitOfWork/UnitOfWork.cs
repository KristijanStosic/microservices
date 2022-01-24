using System.Threading.Tasks;
using DocumentService.DbContext;

namespace DocumentService.Data.UnitOfWork
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