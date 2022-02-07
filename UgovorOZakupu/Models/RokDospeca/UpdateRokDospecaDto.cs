using System;
using System.ComponentModel.DataAnnotations;

namespace UgovorOZakupu.Models.RokDospeca
{
    /// <summary>
    ///     Model roka dospeća za izmenu
    /// </summary>
    public class UpdateRokDospecaDto
    {
        /// <summary>
        ///     Id roka dospeća
        /// </summary>
        [Required(ErrorMessage = "Obavezno je id roka dospeća")]
        public Guid Id { get; set; }

        /// <summary>
        ///     Rok dospeća
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti rok dospeća.")]
        public int Rok { get; set; }

        /// <summary>
        ///     Id ugovora o zakupu
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id ugovora o zakupu.")]
        public Guid UgovorOZakupuId { get; set; }
    }
}