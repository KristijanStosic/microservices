using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities
{
    [Index(nameof(NazivStatusaZalbe), IsUnique = true)]
    public class StatusZalbe
    {
        [Key]
        public Guid StatusZalbeId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivStatusaZalbe { get; set; }
    }
}
