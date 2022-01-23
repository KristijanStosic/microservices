using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Entities
{
    public class Drzava
    {

        [Key]
        public Guid DrzavaId { get; set; } = Guid.NewGuid();
        [Required]
        public string NazivDrzave { get; set; }
    }
}
