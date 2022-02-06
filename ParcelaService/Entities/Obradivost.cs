using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class Obradivost
    {
        [Key]
        public Guid ObradivostId { get; set; } = Guid.NewGuid();

        [Required]
        public string OpisObradivosti { get; set; }
    }
}
