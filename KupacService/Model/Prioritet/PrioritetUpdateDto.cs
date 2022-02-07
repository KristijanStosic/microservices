using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Prioritet
{
    /// <summary>
    /// Dto za ažuriranje prioriteta
    /// </summary>
    public class PrioritetUpdateDto
    {
        /// <summary>
        /// Id prioriteta
        /// </summary>
        [Required]
        public Guid PrioritetId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Opis prioriteta
        /// </summary>
        public string Opis { get; set; }
    }
}
