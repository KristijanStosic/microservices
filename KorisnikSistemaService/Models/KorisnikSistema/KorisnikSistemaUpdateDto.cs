using System;
using System.ComponentModel.DataAnnotations;

namespace KorisnikSistemaService.Models.KorisnikSistema
{
    /// <summary>
    /// Model za izmjenu korisnika sistema
    /// </summary>
    public class KorisnikSistemaUpdateDto
    {
        /// <summary>
        /// ID korisnika sistema
        /// </summary>
        [Required]
        public Guid KorisnikSistemaId { get; set; }
        /// <summary>
        /// Ime korisnika sistema
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti ime korisnika sistema.")]
        public string Ime { get; set; }
        /// <summary>
        /// Prezime korisnika sistema
        /// </summary>
        public string Prezime { get; set; }
        /// <summary>
        /// Email korisnika sistema
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti email korisnika sistema.")]
        public string Email { get; set; }
        /// <summary>
        /// Korisnicko ime korisnika sistema
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti korisnicko ime korisnika sistema.")]
        public string KorisnickoIme { get; set; }
        /// <summary>
        /// Lozinka korisnika sistema
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti lozinku korisnika sistema.")]
        public string Lozinka { get; set; }
        /// <summary>
        /// Tip korisnika sistema
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti tip korisnika sistema.")]
        public Guid TipKorisnikaId { get; set; }
    }
}
