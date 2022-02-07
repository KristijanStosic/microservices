using System;
using System.Threading.Tasks;
using DokumentService.Data.Dokument;
using DokumentService.Data.TipDokumenta;
using DokumentService.DbContext;

namespace DokumentService.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DokumentDbContext _context;
        private bool _disposed;

        public UnitOfWork(DokumentDbContext context)
        {
            _context = context;
            Dokument = new DokumentRepository(context);
            TipDokumenta = new TipDokumentaRepository(context);
        }

        public IDokumentRepository Dokument { get; }
        public ITipDokumentaRepository TipDokumenta { get; }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _context.Dispose();
            _disposed = true;
        }
    }
}