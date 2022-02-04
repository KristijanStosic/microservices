using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Prioritet
{
    public class PrioritetUpdateDto
    {
        [Required]
        public Guid PrioritetId { get; set; } = Guid.NewGuid();
        public string Opis { get; set; }
    }
}
