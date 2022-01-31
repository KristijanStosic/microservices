using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UgovorOZakupu.DbContext;

namespace UgovorOZakupu.Data.RokDospeca
{
    public class RokDospecaRepository : IRokDospecaRepository
    {
        private readonly UgovorOZakupuDbContext _db;

        public RokDospecaRepository(UgovorOZakupuDbContext db)
        {
            _db = db;
        }

        public Task<List<Entities.RokDospeca>> GetAllRokDospeca()
        {
            return _db.RokoviDospeca.ToListAsync();
        }

        public Task<Entities.RokDospeca> GetRokDospecaById(Guid id)
        {
            return _db.RokoviDospeca.FirstOrDefaultAsync(rd => rd.Id == id);
        }

        public void CreateRokDospeca(Entities.RokDospeca rokDospeca)
        {
            _db.Add(rokDospeca);
        }

        public void DeleteRokDospeca(Entities.RokDospeca rokDospeca)
        {
            _db.Remove(rokDospeca);
        }
    }
}