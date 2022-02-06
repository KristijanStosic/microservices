using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Obradivost
{
    /// <summary>
    /// Model za izmenu obradivosti parcele
    /// </summary>
    public class ObradivostUpdateDto
    {
        /// <summary>
        /// Id obradivosti parcele
        /// </summary>
        public Guid ObradivostId { get; set; }
        /// <summary>
        /// Opis obradivosti parcele
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti opis obradivosti!")]
        public string OpisObradivosti { get; set; }

    }
}
