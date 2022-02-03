using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models.Zalba
{
    /// <summary>
    /// Creation Dto za žalbu
    /// </summary>
    public class ZalbaCreateDto : IValidatableObject
    {
        /// <summary>
        /// Datum podnošenja
        /// </summary>
        [Required(ErrorMessage ="Datum podnosenja je obavezno polje.")]
        public DateTime DatumPodnosenja { get; set; }
        /// <summary>
        /// Datum rešenja
        /// </summary>
        [Required(ErrorMessage = "Datum resenja je obavezno polje.")]
        public DateTime DatumResenja { get; set; }
        /// <summary>
        /// Razlog žalbe
        /// </summary>
        [Required(ErrorMessage = "Razlog zalbe je obavezno polje.")]
        public string RazlogZalbe { get; set; }
        /// <summary>
        /// Obrazlozenje
        /// </summary>
        [Required(ErrorMessage = "Obrazlozenje je obavezno polje.")]
        public string Obrazlozenje { get; set; }
        /// <summary>
        /// Broj Nadmetanja
        /// </summary>
        [Required(ErrorMessage = "Broj nadmetanja je obavezno polje.")]
        public string BrojNadmetanja { get; set; }
        /// <summary>
        /// Broj Resenja
        /// </summary>
        [Required(ErrorMessage = "Broj resenja je obavezno polje.")]
        public string BrojResenja { get; set; }
        /// <summary>
        /// Status Zalbe Id
        /// </summary>
        [Required(ErrorMessage = "StatusZalbeId je obavezno polje.")]
        public Guid StatusZalbeId { get; set; }
        /// <summary>
        /// Tip Zalbe Id
        /// </summary>
        [Required(ErrorMessage = "TipZalbeId je obavezno polje.")]
        public Guid TipZalbeId { get; set; }
        /// <summary>
        /// Radnja Za ZalbuId
        /// </summary>
        [Required(ErrorMessage = "RadnjaZaZalbuId je obavezno polje.")]
        public Guid RadnjaZaZalbuId { get; set; }
        /// <summary>
        /// Id kupca - veza sa mikroservisom Kupac
        /// </summary>
        public Guid? KupacId { get; set; }

        /// <summary>
        /// Validacija unetih vrednosti za kreiranje žalbe
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
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
