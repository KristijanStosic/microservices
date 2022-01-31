using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UgovorOZakupu.DbContext;

namespace UgovorOZakupu.Data.UgovorOZakupu
{
    public class UgovorOZakupuRepository : IUgovorOZakupuRepository
    {
        private readonly UgovorOZakupuDbContext _db;

        public UgovorOZakupuRepository(UgovorOZakupuDbContext db)
        {
            _db = db;
        }

        public Task<List<Entities.UgovorOZakupu>> GetAllUgovorOZakupu()
        {
            return _db.UgovoriOZakupu
                .Include(u => u.TipGarancije)
                .Include(u => u.RokoviDospeca)
                .ToListAsync();
        }

        public Task<Entities.UgovorOZakupu> GetUgovorOZakupuById(Guid id)
        {
            return _db.UgovoriOZakupu
                .Include(u => u.TipGarancije)
                .Include(u => u.RokoviDospeca)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public void CreateUgovorOZakupu(Entities.UgovorOZakupu ugovorOZakupu)
        {
            _db.Add(ugovorOZakupu);
        }

        public void DeleteUgovorOZakupu(Entities.UgovorOZakupu ugovorOZakupu)
        {
            _db.Remove(ugovorOZakupu);
        }
    }
}