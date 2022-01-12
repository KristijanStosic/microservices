using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Entities
{
    public class Status
    {
        [Key]
        public Guid StatusId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivStatusa { get; set; }
    }
}
