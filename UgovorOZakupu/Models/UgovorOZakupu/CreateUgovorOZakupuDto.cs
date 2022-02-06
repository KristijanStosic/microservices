using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UgovorOZakupu.Models.UgovorOZakupu
{
    /// <summary>
    ///     Model ugovora o zakupu za kreiranje
    /// </summary>
    public class CreateUgovorOZakupuDto
    {
        /// <summary>
        ///     Zavodni broj ugovora o zakupu
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti zavodni broj ugovora o zakupu.")]
        public string ZavodniBroj { get; set; }

        /// <summary>
        ///     Datum zavođenja ugovora
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti datum roka zavođenja.")]
        public DateTime DatumZavodjenja { get; set; }

        /// <summary>
        ///     Rok za vraćanje ugovora
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti datum roka za vraćanje.")]
        public DateTime RokZaVracanje { get; set; }

        /// <summary>
        ///     Mesto potpisivanja ugovora
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti mesto potpisivanja ugovora.")]
        public string MestoPotpisivanja { get; set; }

        /// <summary>
        ///     Datum potpisavanja ugovora
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti datum postpisivanja ugovora.")]
        public DateTime DatumPotpisivanja { get; set; }

        /// <summary>
        ///     Id tipa garancije
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id tipa garancije.")]
        public Guid TipGarancijeId { get; set; }

        /// <summary>
        ///     Rokovi dospeća
        /// </summary>
        public IEnumerable<int> RokoviDospeca { get; set; }

        /// <summary>
        ///     Id dokumenta (Konačna odluka)
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id dokumenta konačne odluke.")]
        public Guid DokumentId { get; set; } // Odluka

        /// <summary>
        ///     Id javnog nadmentanja
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id javnog nadmetanja.")]
        public Guid JavnoNadmetanjeId { get; set; }

        /// <summary>
        ///     Id kupca (Lice)
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id kupca.")]
        public Guid KupacId { get; set; }

        /// <summary>
        ///     Id ličnosti (Ministar)
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id ličnosti - Ministra.")]
        public Guid LicnostId { get; set; }
    }
}