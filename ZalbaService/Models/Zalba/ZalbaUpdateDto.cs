using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models.Zalba
{
    /// <summary>
    /// Update dto za žalbu
    /// </summary>
    public class ZalbaUpdateDto : IValidatableObject
    {
        /// <summary>
        /// Datum podnošenja
        /// </summary>
        public DateTime DatumPodnosenja { get; set; }
        /// <summary>
        /// Datum resenja
        /// </summary>
        [Required(ErrorMessage = "Datum resenja je obavezno polje.")]
        public DateTime DatumResenja { get; set; }
        /// <summary>
        /// Razlog žalbe
        /// </summary>
        public string RazlogZalbe { get; set; }
        /// <summary>
        /// Obrazloženje
        /// </summary>
        public string Obrazlozenje { get; set; }
        /// <summary>
        /// Broj nadmetanja
        /// </summary>
        public string BrojNadmetanja { get; set; }
        /// <summary>
        /// Broj resenja
        /// </summary>
        public string BrojResenja { get; set; }
        /// <summary>
        /// Status žalbe id - strani kljuc
        /// </summary>
        [Required(ErrorMessage = "StatusZalbeId je obavezno polje.")]
        public Guid StatusZalbeId { get; set; }
        /// <summary>
        /// Tip Zalbe Id - strani kljuc
        /// </summary>
        [Required(ErrorMessage = "TipZalbeId je obavezno polje.")]
        public Guid TipZalbeId { get; set; }
        /// <summary>
        /// Radnja za zalbu Id - strani kljuc
        /// </summary>
        [Required(ErrorMessage = "RadnjaZaZalbuId je obavezno polje.")]
        public Guid RadnjaZaZalbuId { get; set; }

        /// <summary>
        /// Id kupca - veza sa mikroservisom Kupac
        /// </summary>
        public Guid? KupacId { get; set; }

        /// <summary>
        /// Validacija unetih vrednosti za update žalbe
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BrojNadmetanja == BrojResenja)
            {
                yield return new ValidationResult(
                     "Broj nadmetanja i broj resenja ne mogu biti isti.",
                     new[] { "ZalbaUpdateDto" });
            }

            if (DatumPodnosenja == DatumResenja || DatumResenja <= DatumPodnosenja)
            {
                yield return new ValidationResult(
                     "Datum resenja ne moze biti pre datuma podnosenja ili isti.",
                     new[] { "ZalbaUpdateDto" });
            }

            if (DatumPodnosenja > DateTime.Now || DatumResenja > DateTime.Now)
            {
                yield return new ValidationResult(
                     "Datumi ne mogu biti u buducnosti.",
                     new[] { "ZalbaUpdateDto" });
            }
        }
    }
}
