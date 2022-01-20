using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities
{
    public class Zalba
    {
        [Key]
        public Guid ZalbaId { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime DatumPodnosenja { get; set; }

        [Required]
        public DateTime DatumResenja { get; set; }

        [Required]
        public string RazlogZalbe { get; set; }

        [Required]
        public string Obrazlozenje { get; set; }

        [Required]
        public string BrojNadmetanja { get; set; }

        [Required]
        public string BrojResenja { get; set; }

        public Guid StatusZalbeId { get; set; }
        public StatusZalbe StatusZalbe { get; set; }
        public Guid TipZalbeId { get; set; }
        public TipZalbe TipZalbe { get; set; }
        public Guid RadnjaZaZalbuId { get; set; }
        public RadnjaZaZalbu RadnjaZaZalbu { get; set; }
    }
}
