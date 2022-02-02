using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentService.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Data.Dokument
{
    public class DokumentRepository : IDokumentRepository
    {
        private readonly DokumentDbContext _db;

        public DokumentRepository(DokumentDbContext db)
        {
            _db = db;
        }

        public Task<List<Entities.Dokument>> GetAllDokument()
        {
            return _db.Dokumenti
                .AsNoTracking()
                .Include(d => d.TipDokumenta)
                .ToListAsync();
        }

        public Task<Entities.Dokument> GetDokumentById(Guid id)
        {
            return _db.Dokumenti
                .Include(d => d.TipDokumenta)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public void CreateDokument(Entities.Dokument dokument)
        {
            _db.Dokumenti.Add(dokument);
        }

        public void DeleteDokument(Entities.Dokument dokument)
        {
            _db.Dokumenti.Remove(dokument);
        }
    }
}