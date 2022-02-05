using ParcelaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IKatastarskaOpstinaRepository
    {
        Task<List<KatastarskaOpstina>> GetAllKatastarskaOpstina(string nazivKatastarskeOpstine = null);
        Task<KatastarskaOpstina> GetKatastarskaOpstinaById(Guid katastarskaOpstinaId);
        Task<KatastarskaOpstina> CreateKatastarskaOpstina(KatastarskaOpstina katastarskaOpstina);
        Task DeleteKatastarskaOpstina(Guid katastarskaOpstinaId);
        Task SaveChangesAsync();
    }
}
