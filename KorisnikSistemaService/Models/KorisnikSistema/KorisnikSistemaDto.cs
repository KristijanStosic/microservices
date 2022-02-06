using System;
namespace KorisnikSistemaService.Models.KorisnikSistema
{
    public class KorisnikSistemaDto
    {
        /// <summary>
        /// ID korisnika sistema
        /// </summary>
        public Guid KorisnikSistemaId { get; set; }
        /// <summary>
        /// Ime korisnika sistema
        /// </summary>
        public string Ime { get; set; }
        /// <summary>
        /// Prezime korisnika sistema
        /// </summary>
        public string Prezime { get; set; }
        /// <summary>
        /// Email korisnika sistema
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Korisnicko ime korisnika sistema
        /// </summary>
        public string KorisnickoIme { get; set; }
        /// <summary>
        /// Tip korisnika sistema // drugi entitet
        /// </summary>
        public string TipKorisnika { get; set; }
    }
}
