using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Entities
{
    public class Licnost
    {

        [Key]
        public Guid LicnostId { get; set; } = Guid.NewGuid();

        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        public string Funkcija { get; set; }

        public List<Komisija> Komisije { get; set; }
    }
}
