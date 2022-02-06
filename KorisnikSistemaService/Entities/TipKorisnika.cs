using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace KorisnikSistemaService.Entities
{
    [Index(nameof(NazivTipaKorisnika), IsUnique = true)]
    public class TipKorisnika
    {
        [Key]
        public Guid TipKorisnikaId{ get; set; } = Guid.NewGuid();

        [Required]
        public string NazivTipaKorisnika { get; set; }

    }
}
