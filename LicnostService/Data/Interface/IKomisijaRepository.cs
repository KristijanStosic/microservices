using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LicnostService.Entities;
using LicnostService.Entities.Confirmations;

namespace LicnostService.Data
{
    public interface IKomisijaRepository
    {
        
        Task<List<Komisija>> GetAllKomisije(string nazivKomisije = null);

        Task<Komisija> GetKomisijeById(Guid komisijaId);

        Task<KomisijaConfirmation> CreateKomisija(Komisija komisija);

        Task DeleteKomisija(Guid komisijaId);

        Task UpdateKomisija(Komisija komisija);
    }
}


    

