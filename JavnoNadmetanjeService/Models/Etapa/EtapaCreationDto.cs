using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Etapa
{
    /// <summary>
    /// Model za kreiranje etape
    /// </summary>
    public class EtapaCreationDto : IValidatableObject
    {
        /// <summary>
        /// Datum pocetka etape
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti datum pocetka etape.")]
        public DateTime Datum { get; set; }
        /// <summary>
        /// Dan po redu etape u okviru javnog nadmetanja
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti dan po redu za etapu.")]
        public int DanPoRedu { get; set; }
        /// <summary>
        /// Vreme pocetka etape
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti vreme pocetka etape.")]
        public string VremePocetka { get; set; }
        /// <summary>
        /// Vreme kraja etape
        /// </summary>
        public string VremeKraja { get; set; }
        /// <summary>
        /// Da li je etapa zavrsena uspesno
        /// </summary>
        public bool ZavrsenaUspesno { get; set; }
        /// <summary>
        /// ID javnog nadmetanja u okviru kog se nalazi etapa
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id javnog nadmetanja.")]
        public Guid JavnoNadmetanjeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!TimeSpan.TryParse(VremePocetka, out TimeSpan vremePocetkaProvera))
            {
                yield return new ValidationResult(
                     "Vreme pocetka mora biti uneto u formatu hh:mm",
                     new[] { "EtapaCreationDto" });
            }

            if (!TimeSpan.TryParse(VremeKraja, out TimeSpan vremeKrajaProvera))
            {
                yield return new ValidationResult(
                     "Vreme kraja mora biti uneto u formatu hh:mm",
                     new[] { "EtapaCreationDto" });
            }

            if (vremePocetkaProvera >= vremeKrajaProvera)
            {
                yield return new ValidationResult(
                    "Vreme pocetka ne sme biti isto ili posle vremena kraja",
                    new[] { "EtapaCreationDto" });
            }
        }
    }
}
