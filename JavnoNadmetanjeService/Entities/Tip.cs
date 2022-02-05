using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Entities
{
    [Index(nameof(NazivTipa), IsUnique = true)]
    public class Tip
    {
        [Key]
        public Guid TipId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivTipa { get; set; }
    }
}
