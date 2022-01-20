using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    public class StatusZalbeCreateDto
    {
        [Required(ErrorMessage = "Obavezno je uneti naziv statusa zalbe")]
        public string NazivStatusaZalbe { get; set; }
    }
}
