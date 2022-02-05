using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Entities.Confirmations;

namespace ZalbaService.Data.Interfaces
{
    /// <summary>
    /// Interfejs za kreiranje repo-a za žalbu
    /// </summary>
    public interface IZalbaRepository
    {
        /// <summary>
        /// Dobijanje podataka o svim žalbama
        /// </summary>
        /// <returns></returns>
        Task<List<Zalba>> GetAllZalbe();
        /// <summary>
        /// Dobijanje žalbe po id-u
        /// </summary>
        /// <param name="zalbaId">Id žalbe</param>
        /// <returns></returns>
        Task<Zalba> GetZalbaById(Guid zalbaId);
        /// <summary>
        /// Kreiranje žalbe
        /// </summary>
        /// <param name="zalba">Objekat žalbe</param>
        /// <returns></returns>
        Task<ZalbaConfirmation> CreateZalba(Zalba zalba);
        /// <summary>
        /// Brisanje žalbe
        /// </summary>
        /// <param name="zalbaId">Id žalbe</param>
        /// <returns></returns>
        Task DeleteZalba(Guid zalbaId);
        /// <summary>
        /// Modifikacija žalbe
        /// </summary>
        /// <param name="zalba">Objekat žalbe</param>
        /// <returns></returns>
        Task UpdateZalba(Zalba zalba);
        /// <summary>
        /// Provera validnosti za unos žalbe
        /// </summary>
        /// <param name="zalba">Id broja table</param>
        /// <returns></returns>
        Task<bool> IsValidZalba(Zalba zalba);
    }
}
