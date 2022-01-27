using System;
using System.ComponentModel.DataAnnotations;

namespace OvlascenoLiceService.Models.BrojTable
{
    /// <summary>
    /// Creation Dto za broj table
    /// </summary>
    public class BrojTableCreationDto
    {
        /// <summary>
        /// Oznaka table
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti oznaku table.")]
        public string OznakaTable { get; set; }

        /// <summary>
        /// Id ovlašćenog lica - strani ključ
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id ovlascenog lica.")]
        public Guid OvlascenoLiceId { get; set; }
    }
}
