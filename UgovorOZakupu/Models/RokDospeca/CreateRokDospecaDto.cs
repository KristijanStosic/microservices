using System;
using System.ComponentModel.DataAnnotations;

namespace UgovorOZakupu.Models.RokDospeca
{
    /// <summary>
    ///     Model roka dospeća za kreiranje
    /// </summary>
    public class CreateRokDospecaDto
    {
        /// <summary>
        ///     Rok dospeća
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti rok dospeća.")]
        public int Rok { get; set; }

        /// <summary>
        ///     Id ugovora o zakupu
        /// </summary>
        [Required(ErrorMessage = "Obavezno je id ugovora o zakupu.")]
        public Guid UgovorOZakupuId { get; set; }
    }
}