using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UgovorOZakupu.DbContext;

namespace UgovorOZakupu.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private UgovorOZakupuDbContext _db;

        public Repository(UgovorOZakupuDbContext db)
        {
            _db = db;
        }
        
        public Task<List<T>> GetAll()
        {
            return _db.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<T> GetById(Guid id)
        {
            return _db.Set<T>()
                .FindAsync(id)
                .AsTask();
        }

        public void Create(T entity)
        {
            _db.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }
    }
}