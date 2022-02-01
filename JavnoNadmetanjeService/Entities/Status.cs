using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Entities
{
    [Index(nameof(NazivStatusa), IsUnique = true)]
    public class Status
    {
        [Key]
        public Guid StatusId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivStatusa { get; set; }
    }
}
