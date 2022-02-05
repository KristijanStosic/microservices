using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities
{
    /// <summary>
    /// Predstavlja radnju za žalbu
    /// </summary>
    [Index(nameof(NazivRadnjeZaZalbu), IsUnique = true)]
    public class RadnjaZaZalbu
    {
        /// <summary>
        /// ID radnje za žalbu
        /// </summary>
        [Key]
        public Guid RadnjaZaZalbuId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Naziv radnje za žalbu
        /// </summary>
        [Required]
        public string NazivRadnjeZaZalbu { get; set; }
    }
}
