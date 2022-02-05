using System.Threading.Tasks;
using UgovorOZakupu.DbContext;

namespace UgovorOZakupu.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork

    {
        private readonly UgovorOZakupuDbContext _context;

        public UnitOfWork(UgovorOZakupuDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}