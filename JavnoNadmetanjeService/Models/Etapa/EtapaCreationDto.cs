using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Etapa
{
    public class EtapaCreationDto : IValidatableObject
    {
        [Required(ErrorMessage = "Obavezno je uneti datum pocetka etape.")]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti dan po redu za etapu.")]
        public int DanPoRedu { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti vreme pocetka etape.")]
        public string VremePocetka { get; set; }
        public string VremeKraja { get; set; }
        public bool ZavrsenaUspesno { get; set; }
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
