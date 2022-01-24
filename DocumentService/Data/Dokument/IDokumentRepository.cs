using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentService.Data.Dokument
{
    public interface IDokumentRepository
    {
        Task<List<Entities.Dokument>> GetAllDokument();

        Task<Entities.Dokument> GetDokumentById(Guid id);

        void CreateDokument(Entities.Dokument dokument);

        void DeleteDokument(Entities.Dokument dokument);
    }
}