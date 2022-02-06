using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UgovorOZakupu.DbContext;

namespace UgovorOZakupu.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private UgovorOZakupuDbContext _db;

        public Repository(UgovorOZakupuDbContext db)
        {
            _db = db;
        }
        
        public Task<List<TEntity>> GetAll()
        {
            return _db.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<TEntity> GetById(Guid id)
        {
            return _db.Set<TEntity>()
                .FindAsync(id)
                .AsTask();
        }

        public void Create(TEntity entity)
        {
            _db.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _db.Set<TEntity>().Remove(entity);
        }
    }
}