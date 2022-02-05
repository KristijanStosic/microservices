using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace KorisnikSistemaService.Entities
{
    [Index(nameof(KorisnickoIme), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class KorisnikSistema
    {
        [Key]
        public Guid KorisnikSistemaId { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Ime je obavezno")]
        public string Ime { get; set; }
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Email adresa nije validna")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Korisnicko Ime je obavezno")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        public string Lozinka { get; set; }


        public Guid TipKorisnikaId { get; set; }
        public TipKorisnika TipKorisnika { get; set; }
    }
}
