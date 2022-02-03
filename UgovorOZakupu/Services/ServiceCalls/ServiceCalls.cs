using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UgovorOZakupu.Models.Dokument;

namespace UgovorOZakupu.Services.ServiceCalls
{
    public class ServiceCalls : IServiceCalls
    {
        private IConfiguration _configuration;

        private IService<DokumentDto> _dokumentService;

        public ServiceCalls(IConfiguration configuration, IService<DokumentDto> dokumentService)
        {
            _configuration = configuration;
            _dokumentService = dokumentService;
        }

        public Task<DokumentDto> GetDokumentById(Guid id)
        {
            var baseUrl = _configuration.GetValue<string>("Services:Dokument");
            return _dokumentService.SendGetRequest($"{baseUrl}/{id}");
        }
    }
}