using System;
using System.ComponentModel.DataAnnotations;

namespace DokumentService.Models.Dokument
{
    /// <summary>
    /// Model za izmenu dokumenta
    /// </summary>
    public class UpdateDokumentDto
    {
        /// <summary>
        /// Id dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id dokumenta.")]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Zavodni broj dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je zavodni broj.")]
        public string ZavodniBroj { get; set; }

        /// <summary>
        /// Datum zavođenja dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti datum zavođenja dokumenta.")]
        public DateTime Datum { get; set; }

        /// <summary>
        /// Datum donošenja dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je datum donošenja dokumenta.")]
        public DateTime DatumDonosenjaDokumenta { get; set; }

        /// <summary>
        /// Id tipa dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id tipa dokumenta.")]
        public Guid TipDokumentaId { get; set; }
    }
}