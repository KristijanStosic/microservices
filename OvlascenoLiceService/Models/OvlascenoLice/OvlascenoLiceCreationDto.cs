using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OvlascenoLiceService.Models.OvlascenoLice
{
    /// <summary>
    /// Creation Dto za ovlasceno lice
    /// </summary>
    public class OvlascenoLiceCreationDto : IValidatableObject
    {
        /// <summary>
        /// Ime ovlascenog lica
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti ime ovlascenog lica.")]
        public string Ime { get; set; }
        /// <summary>
        /// Prezime ovlascenog lica
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti prezime ovlascenog lica.")]
        public string Prezime { get; set; }
        /// <summary>
        /// JMBG ovlascenog lica - 13 brojeva
        /// </summary>
        [MaxLength(13, ErrorMessage = "JMBG mora da ima 13 brojeva.")]
        [MinLength(13, ErrorMessage = "JMBG mora da ima 13 brojeva.")]
        public string JMBG { get; set; }
        /// <summary>
        /// Broj pasosa ovlascenog lica
        /// </summary>
        public string BrojPasosa { get; set; }

        /// <summary>
        /// Id adrese - veza sa mikroservisom Adresa - adresa se ne unosi za strance
        /// </summary>
        public Guid? AdresaId { get; set; }
        /// <summary>
        /// Id drzave - veza sa mikroservisom Adresa - drzava se unosi za strance
        /// </summary>
        public Guid? DrzavaId { get; set; }

        /// <summary>
        /// Validacija unetih vrednosti za kreiranje ovlascenog lica
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((JMBG is not null && AdresaId is null) ||
                (JMBG is null && AdresaId is not null))
            {
                yield return new ValidationResult(
                    "Ukoliko je ovlasceno lice drzavljanin Srbije morate uneti JMBG i id njegove adrese",
                    new[] { "OvlascenoLiceCreationDto" });
            }

            if ((BrojPasosa is not null && DrzavaId is null) ||
                (BrojPasosa is null && DrzavaId is not null))
            {
                yield return new ValidationResult(
                    "Ukoliko ovlasceno lice nije drzavljanin Srbije morate uneti broj pasosa i id njegove drzave",
                    new[] { "OvlascenoLiceCreationDto" });
            }

            if(BrojPasosa is not null && DrzavaId is not null
                && JMBG is not null && AdresaId is not null)
            {
                yield return new ValidationResult(
                   "Ukoliko ovlasceno lice nije drzavljanin Srbije morate uneti broj pasosa i id njegove drzave, a ako jeste onda unosite samo JMBG i id njegove adrese",
                   new[] { "OvlascenoLiceCreationDto" });
            }
        }
    }
}