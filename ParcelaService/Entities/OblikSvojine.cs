using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class OblikSvojine
    {
        [Key]
        public Guid OblikSvojineId { get; set; } = Guid.NewGuid();
        [Required]
        public string OpisOblikaSvojine { get; set; }
    }
}
