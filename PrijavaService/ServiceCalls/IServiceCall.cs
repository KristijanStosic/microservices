using System.Threading.Tasks;

namespace PrijavaService.ServiceCalls
{
    public interface IServiceCall<T>
    {
        Task<T> SendGetRequestAsync(string url);
    }
}
