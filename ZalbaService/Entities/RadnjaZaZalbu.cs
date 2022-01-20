using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities
{
    [Index(nameof(NazivRadnjeZaZalbu), IsUnique = true)]
    public class RadnjaZaZalbu
    {
        [Key]
        public Guid RadnjaZaZalbuId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivRadnjeZaZalbu { get; set; }
    }
}
