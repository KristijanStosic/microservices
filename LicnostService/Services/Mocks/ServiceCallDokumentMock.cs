using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LicnostService.Models.OtherModels;

namespace LicnostService.Services.Mocks
{
    public class ServiceCallDokumentMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url, string token)
        {
            var dokument = new DokumentDto
            {
                ZavodniBroj = "235",
                TipDokumenta = "Tip 2",
                DatumDonosenjaDokumenta = DateTime.Now
            };

            return await Task.FromResult((T)Convert.ChangeType(dokument, typeof(T)));
        }
    }
}

