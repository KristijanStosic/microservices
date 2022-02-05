using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Entities
{
    public class Etapa
    {
        [Key]
        public Guid EtapaId { get; set; } = Guid.NewGuid();
        [Required]
        public DateTime Datum { get; set; }
        [Required]
        public int DanPoRedu { get; set; }
        [Required]
        public string VremePocetka { get; set; }
        public string VremeKraja { get; set; }
        public bool ZavrsenaUspesno { get; set; }

        public Guid JavnoNadmetanjeId { get; set; }
        public JavnoNadmetanje JavnoNadmetanje { get; set; }
    }
}
