using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.OblikSvojine
{
    /// <summary>
    /// Model za izmenu oblika svojine
    /// </summary>
    public class OblikSvojineUpdateDto
    {
        /// <summary>
        /// Id oblika svojine
        /// </summary>
        public Guid OblikSvojineId { get; set; }
        /// <summary>
        /// Opis oblika svojine
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti oblik svojine!")]
        public string OpisOblikaSvojine { get; set; }
    }
}
