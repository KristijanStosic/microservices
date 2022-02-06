using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UplataService.Entities;

namespace UplataService.Repository
{
    public interface IUplataRepository
    {
        Task<List<Uplata>> GetAllUplate();

        Task<Uplata> GetUplataById(Guid uplataId);

        Task<UplataConfirmation> CreateUplata(Uplata uplata);

        Task DeleteUplata(Guid uplataId);

        Task UpdateUplata(Uplata uplata);

        Task<bool> IsValidUplata(string brojRacuna);
    }
}
