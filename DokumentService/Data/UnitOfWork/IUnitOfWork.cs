using System;
using System.Threading.Tasks;
using DokumentService.Data.Dokument;
using DokumentService.Data.TipDokumenta;

namespace DokumentService.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDokumentRepository Dokument { get; }

        ITipDokumentaRepository TipDokumenta { get; }

        Task CompleteAsync();
    }
}