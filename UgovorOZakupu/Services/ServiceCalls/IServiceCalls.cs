using System;
using System.Threading.Tasks;
using UgovorOZakupu.Models.Dokument;

namespace UgovorOZakupu.Services.ServiceCalls
{
    public interface IServiceCalls
    {
        Task<DokumentDto> GetDokumentById(Guid id);
    }
}