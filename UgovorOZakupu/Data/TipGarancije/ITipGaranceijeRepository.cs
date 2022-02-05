using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UgovorOZakupu.Data.TipGarancije
{
    public interface ITipGaranceijeRepository
    {
        Task<List<Entities.TipGarancije>> GetAllTipGarancije();

        Task<Entities.TipGarancije> GetTipGarancijeById(Guid id);

        void CreateTipGarancije(Entities.TipGarancije tipGarancije);

        void DeleteTipGarancije(Entities.TipGarancije tipGarancije);
    }
}