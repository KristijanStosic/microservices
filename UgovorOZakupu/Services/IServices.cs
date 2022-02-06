using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;
using UgovorOZakupu.Models.Kupac;
using UgovorOZakupu.Models.Licnost;
using UgovorOZakupu.Models.LogModel;

namespace UgovorOZakupu.Services
{
    public interface IServices
    {
        IService<LogModel> Logger { get; }
        IService<DokumentDto> Dokument { get; }
        IService<JavnoNadmetanjeDto> JavnoNadmetanje { get; }
        IService<KupacDto> Kupac { get; }
        IService<LicnostDto> Licnost { get; }
    }
}