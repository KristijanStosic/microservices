using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UgovorOZakupu.Data.RokDospeca
{
    public interface IRokDospecaRepository
    {
        Task<List<Entities.RokDospeca>> GetAllRokDospeca();

        Task<Entities.RokDospeca> GetRokDospecaById(Guid id);
        
        void CreateRokDospeca(Entities.RokDospeca rokDospeca);
        
        void DeleteRokDospeca(Entities.RokDospeca rokDospeca);
    }
}