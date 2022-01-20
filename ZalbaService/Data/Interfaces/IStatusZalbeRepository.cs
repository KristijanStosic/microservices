using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;

namespace ZalbaService.Data
{
    public interface IStatusZalbeRepository 
    {
        Task<List<StatusZalbe>> GetAllStatusesZalbe(string nazivStatusaZalbe = null);
        Task<StatusZalbe> GetStatusZalbeById(Guid statusZalbeId);
        Task<StatusZalbe> CreateStatusZalbe(StatusZalbe statusZalbe);
        Task DeleteStatusZalbe(Guid statusZalbeId);
        Task UpdateStatusZalbe(StatusZalbe statusZalbe);
        Task<bool> IsValidStatusZalbe(string nazivStatusaZalbe);
    }
}
