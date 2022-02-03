using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities
{
    /// <summary>
    /// Predstavlja Tip žalbe
    /// </summary>
    [Index(nameof(NazivTipaZalbe), IsUnique = true)]
    public class TipZalbe
    {
        /// <summary>
        /// ID tipa žalbe
        /// </summary>
        [Key]
        public Guid TipZalbeId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Naziv tipa žalbe
        /// </summary>
        [Required]
        public string NazivTipaZalbe { get; set; }

    }
}
