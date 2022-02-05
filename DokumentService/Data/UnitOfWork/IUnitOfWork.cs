using System.Threading.Tasks;

namespace DokumentService.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}