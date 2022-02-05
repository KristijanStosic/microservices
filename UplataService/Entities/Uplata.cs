using System;
using System.ComponentModel.DataAnnotations;

namespace UplataService.Entities
{
    public class Uplata
    {
        [Key]
        public Guid UplataId { get; set; } = Guid.NewGuid();

        [MaxLength(20)]
        [MinLength(20)]
        public string BrojRacuna { get; set; }
        public string PozivNaBroj { get; set; }

        [Required(ErrorMessage ="Iznos je obavezno polje.")]
        public double Iznos { get; set; }

        [Required(ErrorMessage = "Svrha uplate je obavezno polje.")]
        public string SvrhaUplate { get; set; }

        [Required(ErrorMessage = "Datum uplate je obavezno polje.")]
        public DateTime DatumUplate { get; set; }

        public Kurs Kurs { get; set; }

        public Guid? JavnoNadmetanjeId { get; set; }
    }

    public record Kurs(double VrednostKursa);
}
