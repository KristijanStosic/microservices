using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OvlascenoLiceService.Models.OvlascenoLice
{
    /// <summary>
    /// Update dto za ovlašćeno lice
    /// </summary>
    public class OvlascenoLiceUpdateDto : IValidatableObject
    {
        /// <summary>
        /// ID ovlašćenog lica
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id ovlascenog lica.")]
        public Guid OvlascenoLiceId { get; set; }
        /// <summary>
        /// Ime ovlašćenog lica
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti ime ovlascenog lica.")]
        public string Ime { get; set; }
        /// <summary>
        /// Prezime ovlašćenog lica
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti prezime ovlascenog lica.")]
        public string Prezime { get; set; }
        /// <summary>
        /// JMBG ovlašćenog lica - 13 brojeva
        /// </summary>
        [MaxLength(13, ErrorMessage = "JMBG mora da ima 13 brojeva.")]
        [MinLength(13, ErrorMessage = "JMBG mora da ima 13 brojeva.")]
        public string JMBG { get; set; }
        /// <summary>
        /// Broj pasoša ovlašćenog lica
        /// </summary>
        public string BrojPasosa { get; set; }

        /// <summary>
        /// Id adrese - veza sa mikroservisom Adresa - adresa se ne unosi za strance
        /// </summary>
        public Guid? AdresaId { get; set; }
        /// <summary>
        /// Id drzave - veza sa mikroservisom Adresa - država se unosi za strance
        /// </summary>
        public Guid? DrzavaId { get; set; }

        /// <summary>
        /// Validacija unetih vrednosti za kreiranje ovlašćenog lica
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((JMBG is not null && AdresaId is null) ||
                (JMBG is null && AdresaId is not null))
            {
                yield return new ValidationResult(
                    "Ukoliko je ovlašćeno lice državljanin Srbije morate uneti JMBG i id njegove adrese",
                    new[] { "OvlascenoLiceUpdateDto" });
            }

            if ((BrojPasosa is not null && DrzavaId is null) ||
                (BrojPasosa is null && DrzavaId is not null))
            {
                yield return new ValidationResult(
                    "Ukoliko ovlašćeno lice nije državljanin Srbije morate uneti broj pasosša i id njegove države",
                    new[] { "OvlascenoLiceUpdateDto" });
            }

            if (BrojPasosa is not null && DrzavaId is not null
                && JMBG is not null && AdresaId is not null)
            {
                yield return new ValidationResult(
                   "Ukoliko ovlašćeno lice nije državljanin Srbije morate uneti broj pasosa i id njegove države, a ako jeste onda unosite samo JMBG i id njegove adrese",
                   new[] { "OvlascenoLiceUpdateDto" });
            }
        }
    }
}