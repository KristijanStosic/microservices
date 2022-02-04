using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Parcela
{
    public class ParcelaUpdateDto
    {
        /// <summary>
        /// Id parcele
        /// </summary>
        public Guid ParcelaId { get; set; }
        /// <summary>
        /// Broj parcele
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti broj parcele.")]
        public string BrojParcele { get; set; }
        /// <summary>
        /// Površina parcele
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti površinu parcele.")]
        public string PovrsinaParcele { get; set; }
        /// <summary>
        /// Broj liste nepokretnosti
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti broj liste nepokretnosti.")]
        public string BrojListeNepokretnosti { get; set; }

        /// <summary>
        /// Id zaštićene zone
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id zaštićene zone .")]
        public Guid ZasticenaZonaId { get; set; }
        /// <summary>
        /// Id oblika svojine
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id oblika svojine.")]
        public Guid OblikSvojineId { get; set; }
        /// <summary>
        /// Id odvodnjavanja
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id odvodnjavanja.")]
        public Guid OdvodnjavanjeId { get; set; }
        /// <summary>
        /// Id katastarske opštine
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id katastarske opštine.")]
        public Guid KatastarskaOpstinaId { get; set; }
    }
}
