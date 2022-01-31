using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UgovorOZakupu.DbContext;

namespace UgovorOZakupu.Data.TipGarancije
{
    public class TipGaranceijeRepository : ITipGaranceijeRepository
    {
        private readonly UgovorOZakupuDbContext _db;

        public TipGaranceijeRepository(UgovorOZakupuDbContext db)
        {
            _db = db;
        }
        
        public Task<List<Entities.TipGarancije>> GetAllTipGarancije()
        {
            return _db.TipoviGarancije.ToListAsync();
        }

        public Task<Entities.TipGarancije> GetTipGarancijeById(Guid id)
        {
            return _db.TipoviGarancije.FirstOrDefaultAsync(tg => tg.Id == id);
        }

        public void CreateTipGarancije(Entities.TipGarancije tipGarancije)
        {
            _db.TipoviGarancije.Add(tipGarancije);
        }

        public void DeleteTipGarancije(Entities.TipGarancije tipGarancije)
        {
            _db.TipoviGarancije.Remove(tipGarancije);

        }
    }
}