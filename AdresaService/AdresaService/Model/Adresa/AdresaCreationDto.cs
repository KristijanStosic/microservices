using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Model.Adresa
{
    public class AdresaCreationDto
    {
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Mesto { get; set; }
        public string PostanskiBroj { get; set; }
        [Required(ErrorMessage ="Obavezno uneti id drzave")]
        public Guid DrzavaId { get; set; }
       
    }
}
