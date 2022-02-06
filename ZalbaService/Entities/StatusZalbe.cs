using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities
{
    /// <summary>
    /// Predstavlja status žalbe
    /// </summary>
    [Index(nameof(NazivStatusaZalbe), IsUnique = true)]
    public class StatusZalbe
    {
        /// <summary>
        /// ID statusa žalbe
        /// </summary>
        [Key]
        public Guid StatusZalbeId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Naziv statusa žalbe 
        /// </summary>
        [Required]
        public string NazivStatusaZalbe { get; set; }
    }
}
