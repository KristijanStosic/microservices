using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UgovorOZakupu.DbContext;

namespace UgovorOZakupu.Data.UgovorOZakupu
{
    public class UgovorOZakupuRepository : IRepository<Entities.UgovorOZakupu>
    {
        private readonly UgovorOZakupuDbContext _db;

        public UgovorOZakupuRepository(UgovorOZakupuDbContext db)
        {
            _db = db;
        }

        public Task<List<Entities.UgovorOZakupu>> GetAll()
        {
            return _db.UgovoriOZakupu
                .AsNoTracking()
                .Include(u => u.TipGarancije)
                .Include(u => u.RokoviDospeca)
                .ToListAsync();
        }

        public Task<Entities.UgovorOZakupu> GetById(Guid id)
        {
            return _db.UgovoriOZakupu
                .Include(u => u.TipGarancije)
                .Include(u => u.RokoviDospeca)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Create(Entities.UgovorOZakupu entity)
        {
            _db.Add(entity);
        }

        public void Delete(Entities.UgovorOZakupu entity)
        {
            _db.Remove(entity);
        }
    }
}