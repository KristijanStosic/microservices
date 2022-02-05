using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.DeoParcele
{
    public class DeoParceleUpdateDto
    {
        /// <summary>
        /// Id dela parcele
        /// </summary>
        public Guid DeoParceleId { get; set; }
        /// <summary>
        /// Redni broj dela parcele
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti redni broj dela")]
        public string RedniBrojDela { get; set; }
        /// <summary>
        /// Povrsina dela parcele
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti površinu dela")]
        public string PovrsinaDela { get; set; }

        /// <summary>
        /// Id kulture
        /// </summary>
        public Guid KulturaId { get; set; }
        /// <summary>
        /// Id klase
        /// </summary>
        public Guid KlasaId { get; set; }
        /// <summary>
        /// Id obradivosti
        /// </summary>
        public Guid ObradivostId { get; set; }
        /// <summary>
        /// Id parcele
        /// </summary>
        public Guid ParcelaId { get; set; }


        /// <summary>
        /// Id kupca
        /// </summary>
        public Guid? KupacId { get; set; }
    }
}
