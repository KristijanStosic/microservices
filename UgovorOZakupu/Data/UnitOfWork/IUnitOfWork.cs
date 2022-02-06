using System;
using System.Threading.Tasks;
using UgovorOZakupu.Entities;

namespace UgovorOZakupu.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TipGarancije> TipoviGarancije { get; }
        IRepository<RokDospeca> RokoviDospeca { get; }
        IRepository<Entities.UgovorOZakupu> UgovoriOZakupu { get; }
        Task CompleteAsync();
    }
}