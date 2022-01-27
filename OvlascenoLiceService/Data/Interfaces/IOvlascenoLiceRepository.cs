using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OvlascenoLiceService.Data.Interfaces
{
    /// <summary>
    /// Interfejs za kreiranje repo-a za ovlasceno lice
    /// </summary>
    public interface IOvlascenoLiceRepository
    {
        /// <summary>
        /// Dobijanje podataka o svim ovlascenim licima
        /// </summary>
        /// <returns></returns>
        Task<List<OvlascenoLice>> GetAllOvlascenoLice();
        /// <summary>
        /// Dobijanje ovlascenog lica po id-u
        /// </summary>
        /// <param name="ovlascenoLiceId">Id ovlascenog lica</param>
        /// <returns></returns>
        Task<OvlascenoLice> GetOvlascenoLiceById(Guid ovlascenoLiceId);
        /// <summary>
        /// Dobijanje ovlascenih lica po imenu ili prezimenu
        /// </summary>
        /// <param name="ime">Ime ovlascenog lica</param>
        /// <param name="prezime">Prezime ovlascenog lica</param>
        /// <returns></returns>
        Task<List<OvlascenoLice>> GetOvlascenaLicaByImePrezime(string ime = null, string prezime = null);
        /// <summary>
        /// Kreiranje ovlascenog lica
        /// </summary>
        /// <param name="ovlascenoLice">Objekat ovlasceno lice</param>
        /// <returns></returns>
        Task<OvlascenoLiceConfirmation> CreateOvlascenoLice(OvlascenoLice ovlascenoLice);
        /// <summary>
        /// Brisanje ovlascenog lica
        /// </summary>
        /// <param name="ovlascenoLiceId">Id ovlascenog lica</param>
        /// <returns></returns>
        Task DeleteOvlascenoLice(Guid ovlascenoLiceId);
        /// <summary>
        /// Cuvanje izmena u bazi podataka
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
