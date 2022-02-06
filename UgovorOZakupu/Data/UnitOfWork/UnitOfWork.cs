using System.Threading.Tasks;
using UgovorOZakupu.Data.UgovorOZakupu;
using UgovorOZakupu.DbContext;

namespace UgovorOZakupu.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UgovorOZakupuDbContext _context;
        
        public UnitOfWork(UgovorOZakupuDbContext context)
        {
            _context = context;
            TipoviGarancije = new Repository<Entities.TipGarancije>(context);
            RokoviDospeca = new Repository<Entities.RokDospeca>(context);
            UgovoriOZakupu = new UgovorOZakupuRepository(context);
        }

        public IRepository<Entities.TipGarancije> TipoviGarancije { get; }
        
        public IRepository<Entities.RokDospeca> RokoviDospeca { get; }
        
        public IRepository<Entities.UgovorOZakupu> UgovoriOZakupu { get; }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}