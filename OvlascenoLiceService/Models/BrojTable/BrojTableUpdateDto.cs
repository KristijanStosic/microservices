using System;
using System.ComponentModel.DataAnnotations;

namespace OvlascenoLiceService.Models.BrojTable
{
    /// <summary>
    /// Update Dto za broj table
    /// </summary>
    public class BrojTableUpdateDto
    {
        /// <summary>
        /// ID broja table
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id broja table.")]
        public Guid BrojTableId { get; set; }
        /// <summary>
        /// Oznaka table
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti oznaku table.")]
        public string OznakaTable { get; set; }

        /// <summary>
        /// Id ovlascenog lica - strani kljuc
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id ovlascenog lica.")]
        public Guid OvlascenoLiceId { get; set; }
    }
}
