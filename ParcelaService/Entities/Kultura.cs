using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class Kultura
    {
        [Key]
        public Guid KulturaId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivKulture { get; set; }
    }
}
