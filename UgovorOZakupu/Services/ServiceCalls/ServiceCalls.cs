using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;
using UgovorOZakupu.Models.UgovorOZakupu;

namespace UgovorOZakupu.Services.ServiceCalls
{
    public class ServiceCalls : IServiceCalls
    {
        private readonly IConfiguration _configuration;

        private readonly IService<DokumentDto> _dokumentService;
        private readonly IService<JavnoNadmetanjeDto> _javnoNadmetanjeService;
        private readonly IMapper _mapper;

        public ServiceCalls(IConfiguration configuration, IMapper mapper, IService<DokumentDto> dokumentService,
            IService<JavnoNadmetanjeDto> javnoNadmetanjeService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _dokumentService = dokumentService;
            _javnoNadmetanjeService = javnoNadmetanjeService;
        }

        public async Task<UgovorOZakupuDto> GetUgovorOZakupuInfo(Entities.UgovorOZakupu ugovor)
        {
            var ugovorDto = _mapper.Map<UgovorOZakupuDto>(ugovor);

            ugovorDto.Dokument = await GetDokumentById(ugovor.DokumentId);
            ugovorDto.JavnoNadmetanje = await GetJavnoNadmentanjeById(ugovor.JavnoNadmetanjeId);

            return ugovorDto;
        }

        private Task<DokumentDto> GetDokumentById(Guid id)
        {
            var baseUrl = _configuration.GetValue<string>("Services:Dokument");
            return _dokumentService.SendGetRequest($"{baseUrl}/{id}");
        }

        private Task<JavnoNadmetanjeDto> GetJavnoNadmentanjeById(Guid id)
        {
            var baseUrl = _configuration.GetValue<string>("Services:JavnoNadmetanje");
            return _javnoNadmetanjeService.SendGetRequest($"{baseUrl}/{id}");
        }
    }
}