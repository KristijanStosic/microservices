using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.JavnoNadmetanje
{
    public class JavnoNadmetanjeUpdateDto : IValidatableObject
    {
        public Guid JavnoNadmetanjeId { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti pocetnu cenu hektara.")]
        public double PocetnaCenaHektar { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti period zakupa.")]
        public int PeriodZakupa { get; set; }
        public int IzlicitiranaCena { get; set; }
        public int BrojUcesnika { get; set; }
        public int Krug { get; set; }
        public bool Izuzeto { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti id statusa javnog nadmetanja.")]
        public Guid StatusId { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti id tipa javnog nadmetanja.")]
        public Guid TipId { get; set; }
        public Guid? KupacId { get; set; }
        public Guid? AdresaId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IzlicitiranaCena < PocetnaCenaHektar)
            {
                yield return new ValidationResult(
                     "Izlicitirana cena mora biti veca od pocetne cene!",
                     new[] { "JavnoNadmetanjeUpdateDto" });
            }
        }
    }
}
