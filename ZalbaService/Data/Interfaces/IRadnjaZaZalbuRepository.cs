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
    public interface IRadnjaZaZalbuRepository
    {
        // <summary>
        /// Dobijanje podataka o svim radnjama za žalbu
        /// </summary>
        /// <returns></returns>
        Task<List<RadnjaZaZalbu>> GetAllRadnjeZaZalbu(string nazivRadnjeZaZalbu = null);
        /// <summary>
        /// Dobijanje radnje za žalbu po id-u
        /// </summary>
        /// <param name="radnjaZaZalbuId">Id radnje za žalbu</param>
        /// <returns></returns>
        Task<RadnjaZaZalbu> GetRadnjaZaZalbuById(Guid radnjaZaZalbuId);
        /// <summary>
        /// Kreiranje radnje za žalbu
        /// </summary>
        /// <param name="radnjaZaZalbu">Objekat radnja za žalbu</param>
        /// <returns></returns>
        Task<RadnjaZaZalbu> CreateRadnjaZaZalbu(RadnjaZaZalbu radnjaZaZalbu);
        /// <summary>
        /// Brisanje radnje za žalbu
        /// </summary>
        /// <param name="radnjaZaZalbuId">Id radnje za žalbu</param>
        /// <returns></returns>
        Task DeleteRadnjaZaZalbu(Guid radnjaZaZalbuId);
        /// <summary>
        /// Modifikacija radnje za žalbu
        /// </summary>
        /// <param name="radnjaZaZalbu">Objekat radnja za žalbu</param>
        /// <returns></returns>
        Task UpdateRadnjaZaZalbu(RadnjaZaZalbu radnjaZaZalbu);
        /// <summary>
        /// Provera validnosti za unos radnje za žalbu
        /// </summary>
        /// <param name="nazivRadnjeZaZalbu">Id radnje za žalbu/param>
        /// <returns></returns>
        Task<bool> IsValidRadnjaZaZalbu(string nazivRadnjeZaZalbu);
    }
}
