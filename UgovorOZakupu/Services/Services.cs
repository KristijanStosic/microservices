using Microsoft.Extensions.Configuration;
using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;
using UgovorOZakupu.Models.Kupac;
using UgovorOZakupu.Models.Licnost;
using UgovorOZakupu.Models.LogModel;

namespace UgovorOZakupu.Services
{
    public class Services : IServices
    {
        public Services(IConfiguration configuration)
        {
            Logger = new Service<LogModel>(configuration.GetValue<string>("Services:Logger"));
            Dokument = new Service<DokumentDto>(configuration.GetValue<string>("Services:Dokument"));
            JavnoNadmetanje =
                new Service<JavnoNadmetanjeDto>(configuration.GetValue<string>("Services:JavnoNadmetanje"));
            Kupac = new Service<KupacDto>(configuration.GetValue<string>("Services:Kupac"));
            Licnost = new Service<LicnostDto>(configuration.GetValue<string>("Services:Licnost"));
        }

        public IService<LogModel> Logger { get; }
        public IService<DokumentDto> Dokument { get; }
        public IService<JavnoNadmetanjeDto> JavnoNadmetanje { get; }
        public IService<KupacDto> Kupac { get; }
        public IService<LicnostDto> Licnost { get; }
    }
}