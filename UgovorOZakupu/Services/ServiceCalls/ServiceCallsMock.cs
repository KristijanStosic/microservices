using System;
using System.Threading.Tasks;
using UgovorOZakupu.Models.Dokument;

namespace UgovorOZakupu.Services.ServiceCalls
{
    public class ServiceCallsMock : IServiceCalls
    {
        public Task<DokumentDto> GetDokumentById(Guid id)
        {
            var dokument = new DokumentDto
            {
                ZavodniBroj = "IJEN-1/2022",
                Datum = DateTime.Now,
                DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                TipDokumenta = "Izvod iz javne evidencije o nepokretnosti"
            };
            
            return Task.FromResult(dokument);
        }
    }
}