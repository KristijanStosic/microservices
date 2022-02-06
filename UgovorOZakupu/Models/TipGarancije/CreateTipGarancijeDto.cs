using System.ComponentModel.DataAnnotations;

namespace UgovorOZakupu.Models.TipGarancije
{
    /// <summary>
    ///     Model tipa garancije za kreiranje
    /// </summary>
    public class CreateTipGarancijeDto
    {
        /// <summary>
        ///     Naziv tipa garancije
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa garancije.")]
        public string NazivTipa { get; set; }
    }
}