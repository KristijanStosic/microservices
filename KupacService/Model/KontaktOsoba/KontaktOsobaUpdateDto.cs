using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.KontaktOsoba
{
    public class KontaktOsobaUpdateDto
    {
        [Required]
        public Guid KontaktOsobaId { get; set; } = Guid.NewGuid();
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public string Funkcija { get; set; }
    }
}
