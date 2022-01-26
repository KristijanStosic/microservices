using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models.Zalba
{
    public class ZalbaCreateDto : IValidatableObject
    {
        [Required(ErrorMessage ="Datum podnosenja je obavezno polje.")]
        public DateTime DatumPodnosenja { get; set; }

        [Required(ErrorMessage = "Datum resenja je obavezno polje.")]
        public DateTime DatumResenja { get; set; }

        [Required(ErrorMessage = "Razlog zalbe je obavezno polje.")]
        public string RazlogZalbe { get; set; }

        [Required(ErrorMessage = "Obrazlozenje je obavezno polje.")]
        public string Obrazlozenje { get; set; }

        [Required(ErrorMessage = "Broj nadmetanja je obavezno polje.")]
        public string BrojNadmetanja { get; set; }
        [Required(ErrorMessage = "Broj resenja je obavezno polje.")]
        public string BrojResenja { get; set; }

        [Required(ErrorMessage = "StatusZalbeId je obavezno polje.")]
        public Guid StatusZalbeId { get; set; }
        [Required(ErrorMessage = "TipZalbeId je obavezno polje.")]
        public Guid TipZalbeId { get; set; }
        [Required(ErrorMessage = "RadnjaZaZalbuId je obavezno polje.")]
        public Guid RadnjaZaZalbuId { get; set; }

        public Guid? KupacId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(BrojNadmetanja == BrojResenja)
            {
                yield return new ValidationResult(
                     "Broj nadmetanja i broj resenja ne mogu biti isti.",
                     new[] { "ZalbaCreateDto" });
            }

            if(DatumPodnosenja == DatumResenja || DatumResenja <= DatumPodnosenja)
            {
                yield return new ValidationResult(
                     "Datum resenja ne moze biti pre datuma podnosenja ili isti.",
                     new[] { "ZalbaCreateDto" });
            }

            if(DatumPodnosenja > DateTime.Now || DatumResenja > DateTime.Now)
            {
                yield return new ValidationResult(
                     "Datumi ne mogu biti u buducnosti.",
                     new[] { "ZalbaCreateDto" });
            }
        }
    }
}
