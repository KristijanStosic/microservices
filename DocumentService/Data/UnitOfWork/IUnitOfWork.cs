using System.Threading.Tasks;

namespace DocumentService.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}