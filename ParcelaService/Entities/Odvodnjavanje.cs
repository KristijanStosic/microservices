using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class Odvodnjavanje
    {
        [Key]
        public Guid OdvodnjavanjeId { get; set; } = Guid.NewGuid();
        [Required]
        public string OpisOdvodnjavanja { get; set; }
    }
}
