using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;

namespace ZalbaService.Data
{
    /// <summary>
    /// Interfejs za kreiranje repo-a za tip žalbe
    /// </summary>
    public interface ITipZalbeRepository
    {
        /// <summary>
        /// Dobijanje podataka o svim tipovima žalbi
        /// </summary>
        /// <returns></returns>
        Task<List<TipZalbe>> GetAllTipoviZalbe(string nazivTipaZalbe = null);
        /// <summary>
        /// Dobijanje tipa žalbe po id-u
        /// </summary>
        /// <param name="tipZalbeId">Id broja table</param>
        /// <returns></returns>
        Task<TipZalbe> GetTipZalbeById(Guid tipZalbeId);
        /// <summary>
        /// Kreiranje tipa žalbe
        /// </summary>
        /// <param name="tipZalbe">Objekat tip žalbe</param>
        /// <returns></returns>
        Task<TipZalbe> CreateTipZalbe(TipZalbe tipZalbe);
        /// <summary>
        /// Brisanje tipa žalbe
        /// </summary>
        /// <param name="tipZalbeId">Id tipa žalbe</param>
        /// <returns></returns>
        Task DeleteTipZalbe(Guid tipZalbeId);
        /// <summary>
        /// Modifikacija tipa žalbe
        /// </summary>
        /// <param name="tipZalbe">Objekat tip žalbe</param>
        /// <returns></returns>
        Task UpdateTipZalbe(TipZalbe tipZalbe);
        /// <summary>
        /// Provera validnosti za unos tipa žalbe
        /// </summary>
        /// <param name="nazivTipaZalbe">Id broja table</param>
        /// <returns></returns>
        Task<bool> IsValidTipZalbe(string nazivTipaZalbe);
    }
}
