using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Entities
{
    public class Tip
    {
        [Key]
        public Guid TipId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivTipa { get; set; }
    }
}
