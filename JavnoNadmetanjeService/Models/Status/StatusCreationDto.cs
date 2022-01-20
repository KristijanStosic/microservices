using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Status
{
    public class StatusCreationDto
    {
        [Required(ErrorMessage = "Obavezno je uneti naziv statusa javnog nadmetanja")]
        public string NazivStatusa { get; set; }
    }
}
