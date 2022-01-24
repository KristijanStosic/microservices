using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentService.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Data.TipDokumenta
{
    public class TipDokumentaRepository : ITipDokumentaRepository
    {
        private readonly DokumentDbContext _db;

        public TipDokumentaRepository(DokumentDbContext db)
        {
            _db = db;
        }

        public async Task<List<Entities.TipDokumenta>> GetAllTipDokumenta()
        {
            return await _db.TipoviDokumenta.ToListAsync();
        }

        public async Task<Entities.TipDokumenta> GetTipDokumentaById(Guid id)
        {
            return await _db.TipoviDokumenta.FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public void CreateTipDokumenta(Entities.TipDokumenta tipDokumenta)
        {
            _db.TipoviDokumenta.Add(tipDokumenta);
        }

        public void DeleteTipDokumenta(Entities.TipDokumenta tipDokumenta)
        {
            _db.TipoviDokumenta.Remove(tipDokumenta);
        }
    }
}