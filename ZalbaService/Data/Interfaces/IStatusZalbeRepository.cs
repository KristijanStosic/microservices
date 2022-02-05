using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;

namespace ZalbaService.Data
{
    /// <summary>
    /// Interfejs za kreiranje repo-a za status žalbe
    /// </summary>
    public interface IStatusZalbeRepository 
    {
        // <summary>
        /// Dobijanje podataka o svim statusima žalbi
        /// </summary>
        /// <returns></returns>
        Task<List<StatusZalbe>> GetAllStatusesZalbe(string nazivStatusaZalbe = null);
        /// <summary>
        /// Dobijanje statusa žalbe po id-u
        /// </summary>
        /// <param name="statusZalbeId">Id statusa žalbe</param>
        /// <returns></returns>
        Task<StatusZalbe> GetStatusZalbeById(Guid statusZalbeId);
        /// <summary>
        /// Kreiranje statusa žalbe
        /// </summary>
        /// <param name="statusZalbe">Objekat statusa žalbe</param>
        /// <returns></returns>
        Task<StatusZalbe> CreateStatusZalbe(StatusZalbe statusZalbe);
        /// <summary>
        /// Brisanje statusa žalbe
        /// </summary>
        /// <param name="statusZalbeId">Id statusa žalbe</param>
        /// <returns></returns>
        Task DeleteStatusZalbe(Guid statusZalbeId);
        /// <summary>
        /// Modifikacija statusa žalbe
        /// </summary>
        /// <param name="statusZalbe">Objekat statusa žalbe</param>
        /// <returns></returns>
        Task UpdateStatusZalbe(StatusZalbe statusZalbe);
        /// <summary>
        /// Provera validnosti za unos statusa žalbe
        /// </summary>
        /// <param name="nazivStatusaZalbe">Id statusa žalbe/param>
        /// <returns></returns>
        Task<bool> IsValidStatusZalbe(string nazivStatusaZalbe);
    }
}
