using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Tip
{
    public class TipCreationDto 
    {
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa javnog nadmetanja")]
        public string NazivTipa { get; set; }
    }
}
