using System.Threading.Tasks;

namespace UgovorOZakupu.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}