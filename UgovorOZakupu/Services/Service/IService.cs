using System.Threading.Tasks;

namespace UgovorOZakupu.Services.Service
{
    public interface IService<TResponse>
    {
        Task<TResponse> SendGetRequest(string relativePath = "", string token = "");
        Task<TResponse> SendPostRequest<TPayload>(TPayload payload, string relativePath = "", string token = "");
    }
}