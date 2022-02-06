using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UgovorOZakupu.Data
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        
        Task<T> GetById(Guid id);
        
        void Create(T entity);
        
        void Delete(T entity);
    }
}