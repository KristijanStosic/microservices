using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UgovorOZakupu.Data.UgovorOZakupu
{
    public interface IUgovorOZakupuRepository
    {
        Task<List<Entities.UgovorOZakupu>> GetAllUgovorOZakupu();

        Task<Entities.UgovorOZakupu> GetUgovorOZakupuById(Guid id);

        void CreateUgovorOZakupu(Entities.UgovorOZakupu ugovorOZakupu);

        void DeleteUgovorOZakupu(Entities.UgovorOZakupu ugovorOZakupu);
    }
}