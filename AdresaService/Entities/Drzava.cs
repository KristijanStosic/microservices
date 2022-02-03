using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Entities
{
    /// <summary>
    /// Predstavlja državu
    /// </summary>
    public class Drzava
    {
        /// <summary>
        /// Id države
        /// </summary>
        [Key]
        public Guid DrzavaId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Naziv države
        /// </summary>
        [Required]
        public string NazivDrzave { get; set; }
    }
}
