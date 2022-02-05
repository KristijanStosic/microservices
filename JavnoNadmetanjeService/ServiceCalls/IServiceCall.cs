using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls
{
    public interface IServiceCall<T>
    {
        Task<T> SendGetRequestAsync(string url);
    }
}
