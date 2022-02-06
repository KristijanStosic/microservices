using System;
using System.Threading.Tasks;

namespace UgovorOZakupu.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Entities.TipGarancije> TipoviGarancije { get; }
        
        IRepository<Entities.RokDospeca> RokoviDospeca { get; }
        
        IRepository<Entities.UgovorOZakupu> UgovoriOZakupu { get; }

        Task CompleteAsync();
    }
}