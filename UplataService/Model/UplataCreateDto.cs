using System;
using System.ComponentModel.DataAnnotations;
using UplataService.Entities;

namespace UplataService.Model
{
    /// <summary>
    /// Create dto za uplatu
    /// </summary>
    public class UplataCreateDto
    {
        /// <summary>
        /// Broj racuna 
        /// </summary>
        [MaxLength(20, ErrorMessage = "Broj racuna mora biti u formatu XXX-XXXXXXXXXXXXX-XX")]
        [MinLength(20, ErrorMessage = "Broj racuna mora biti u formatu XXX-XXXXXXXXXXXXX-XX")]
        public string BrojRacuna { get; set; }
        /// <summary>
        /// Poziv na broj
        /// </summary>
        public string PozivNaBroj { get; set; }
        /// <summary>
        /// Iznos
        /// </summary>
        [Required(ErrorMessage = "Iznos je obavezno polje.")]
        public double Iznos { get; set; }
        /// <summary>
        /// Svrha uplate
        /// </summary>
        [Required(ErrorMessage = "Svrha uplate je obavezno polje.")]
        public string SvrhaUplate { get; set; }
        /// <summary>
        /// Datum uplate
        /// </summary>
        [Required(ErrorMessage = "Datum uplate je obavezno polje.")]
        public DateTime DatumUplate { get; set; }
        /// <summary>
        /// Id javnog nadmetanja - veza sa mikroservisom Javno nadmetanje
        /// </summary>
        public Guid? JavnoNadmetanjeId { get; set; }
        /// <summary>
        /// Kurs
        /// </summary>
        public Kurs Kurs { get; set; }
    }
}
