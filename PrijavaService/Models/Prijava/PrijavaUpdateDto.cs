using PrijavaService.Models.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.Prijava
{
    /// <summary>
    /// Model izmjene prijave
    /// </summary>
    public class PrijavaUpdateDto
    {
        /// <summary>
        /// ID prijave
        /// </summary>
        public Guid PrijavaId { get; set; }

        /// <summary>
        /// Broj prijave
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti broj prijave.")]
        public string BrojPrijave { get; set; }

        /// <summary>
        /// Datum podnosenja prijave
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti datum prijave.")]
        public DateTime DatumPrijave { get; set; }
        /// <summary>
        /// Mjesto prijema prijave
        /// </summary>
        public string MestoPrijave { get; set; }
        /// <summary>
        /// Tacan sat prijema prijave
        /// </summary>
        public string SatPrijema { get; set; }
        /// <summary>
        /// Oznaka da li je prijava zatvorena
        /// </summary>
        public bool ZatvorenaPonuda { get; set; }
        /// <summary>
        /// Kupac u prijavi
        /// </summary>
        public KupacDto Kupac { get; set; }


        public List<Guid> JavnoNadmetanje { get; set; }
    }
}
