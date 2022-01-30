using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.OblikSvojine
{
    public class OblikSvojineUpdateDto
    {
        public Guid OblikSvojineId { get; set; }
        [Required(ErrorMessage ="Obavezno je uneti oblik svojine!")]
        public string OpisOblikaSvojine { get; set; }
    }
}
