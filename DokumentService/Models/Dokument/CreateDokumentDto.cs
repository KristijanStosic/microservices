using System;
using System.ComponentModel.DataAnnotations;

namespace DokumentService.Models.Dokument
{
    /// <summary>
    ///     Model za kreiranje dokumenta
    /// </summary>
    public class CreateDokumentDto
    {
        /// <summary>
        ///     Zavodni broj dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti zavodni broj.")]
        public string ZavodniBroj { get; set; }

        /// <summary>
        ///     Datum zavođenja dokumenta
        /// </summary>
        public DateTime Datum { get; set; }

        /// <summary>
        ///     Datum donošenja dokumenta
        /// </summary>
        public DateTime? DatumDonosenjaDokumenta { get; set; }

        /// <summary>
        ///     Tip dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je id tipa dokumenta.")]
        public Guid TipDokumentaId { get; set; }
    }
}