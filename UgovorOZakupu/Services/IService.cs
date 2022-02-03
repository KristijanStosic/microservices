using System.Threading.Tasks;

namespace UgovorOZakupu.Services
{
    public interface IService<TResponse>
    {
        Task<TResponse> SendGetRequest(string uri);
        
        Task<TResponse> SendPostRequest<TPayload>(string uri, TPayload payload);
    }
}