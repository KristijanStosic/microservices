using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OvlascenoLiceService.Data.Interfaces
{
    /// <summary>
    /// Interfejs za kreiranje repo-a za broj table
    /// </summary>
    public interface IBrojTableRepository
    {
        /// <summary>
        /// Dobijanje podataka o svim brojevima table
        /// </summary>
        /// <returns></returns>
        Task<List<BrojTable>> GetAllBrojTable();
        /// <summary>
        /// Dobijanje broja table po id-u
        /// </summary>
        /// <param name="brojTableId">Id broja table</param>
        /// <returns></returns>
        Task<BrojTable> GetBrojTableById(Guid brojTableId);
        /// <summary>
        /// Dobijanje brojeva table po obelezju oznaka table
        /// </summary>
        /// <param name="oznakaTable">Oznaka table</param>
        /// <returns></returns>
        Task<List<BrojTable>> GetBrojTableByOznakaTable(string oznakaTable);
        /// <summary>
        /// Kreiranje broja table
        /// </summary>
        /// <param name="brojTable">Objekat broj table</param>
        /// <returns></returns>
        Task<BrojTableConfirmation> CreateBrojTable(BrojTable brojTable);
        /// <summary>
        /// Brisanje broja table
        /// </summary>
        /// <param name="brojTableId">Id broja table</param>
        /// <returns></returns>
        Task DeleteBrojTable(Guid brojTableId);
        /// <summary>
        /// Cuvanje izmena u bazi podataka
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
