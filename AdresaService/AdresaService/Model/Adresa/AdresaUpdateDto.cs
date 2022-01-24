using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Model.Adresa
{
    public class AdresaUpdateDto
    {
        [Required]
        public Guid AdresaId { get; set; }
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Mesto { get; set; }
        public string PostanskiBroj { get; set; }
        [Required]
        public Guid DrzavaId { get; set; }
     
    }
}
