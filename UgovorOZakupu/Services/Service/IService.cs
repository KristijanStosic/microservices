using System.Threading.Tasks;

namespace UgovorOZakupu.Services.Service
{
    public interface IService<TResponse>
    {
        Task<TResponse> SendGetRequest(string token, string uri = "");
        Task<TResponse> SendPostRequest<TPayload>(TPayload payload, string uri = "");
    }
}