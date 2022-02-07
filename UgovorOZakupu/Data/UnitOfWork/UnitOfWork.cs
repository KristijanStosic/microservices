using System;
using System.Threading.Tasks;
using UgovorOZakupu.Data.UgovorOZakupu;
using UgovorOZakupu.DbContext;
using UgovorOZakupu.Entities;

namespace UgovorOZakupu.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UgovorOZakupuDbContext _context;
        private bool _disposed;

        public UnitOfWork(UgovorOZakupuDbContext context)
        {
            _context = context;
            TipoviGarancije = new Repository<TipGarancije>(context);
            RokoviDospeca = new Repository<RokDospeca>(context);
            UgovoriOZakupu = new UgovorOZakupuRepository(context);
        }

        public IRepository<TipGarancije> TipoviGarancije { get; }
        public IRepository<RokDospeca> RokoviDospeca { get; }
        public IRepository<Entities.UgovorOZakupu> UgovoriOZakupu { get; }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _context.Dispose();
            _disposed = true;
        }
    }
}