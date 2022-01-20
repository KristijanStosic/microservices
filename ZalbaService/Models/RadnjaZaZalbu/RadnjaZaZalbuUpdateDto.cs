using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    public class RadnjaZaZalbuUpdateDto
    {
        //public Guid RadnjaZaZalbuId { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Obavezno je uneti naziv radnje za zalbu")]
        public string NazivRadnjeZaZalbu { get; set; }
    }
}
