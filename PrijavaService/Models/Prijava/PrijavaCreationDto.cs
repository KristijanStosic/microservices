using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Models.Prijava
{
    /// <summary>
    /// Model za potvrdu kreiranja prijave
    /// </summary>
    public class PrijavaCreationDto
    {
        /// <summary>
        /// Registarski broj prijave
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti broj prijave.")]
        public string BrojPrijave { get; set; }
        /// <summary>
        /// Datum kada je prijava podnijeta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti datum prijave.")]
        public DateTime DatumPrijave { get; set; }
        /// <summary>
        /// Mjesto u kojem je podnijeta prijava
        /// </summary>
        public string MestoPrijave { get; set; }
        /// <summary>
        /// Sat u kojem je primljena prijava
        /// </summary>
        public string SatPrijema { get; set; }
        /// <summary>
        /// True ili False vrijedno da li je priva zavrsena
        /// </summary>
        public bool ZatvorenaPonuda { get; set; }
        /// <summary>
        /// ID kupca koji podnisi prijavu
        /// </summary>
        public Guid? KupacId { get; set; }

        /// <summary>
        /// Lista javnih nadmenjaa za koje je podnesena prijava
        /// </summary>
        public List<Guid> JavnoNadmetanje { get; set; }
    }
}
