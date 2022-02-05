using System.ComponentModel.DataAnnotations;

namespace DokumentService.Models.TipDokumenta
{
    /// <summary>
    /// Model za kreiranje tipa dokumenta
    /// </summary>
    public class CreateTipDokumentaDto
    {
        /// <summary>
        /// Naziv tipa dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je naziv tipa dokumenta.")]
        public string NazivTipa { get; set; }
    }
}