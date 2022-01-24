using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentService.Data.TipDokumenta
{
    public interface ITipDokumentaRepository
    {
        Task<List<Entities.TipDokumenta>> GetAllTipDokumenta();

        Task<Entities.TipDokumenta> GetTipDokumentaById(Guid id);

        void CreateTipDokumenta(Entities.TipDokumenta tipDokumenta);

        void DeleteTipDokumenta(Entities.TipDokumenta tipDokumenta);
    }
}