using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class DeoParcele
    {
        [Key]
        public Guid DeoParceleId { get; set; } = Guid.NewGuid();
        [Required]
        public string RedniBrojDela { get; set; }
        [Required]
        public string PovrsinaDela { get; set; }

        public Guid KulturaId { get; set; }
        public Kultura Kultura { get; set; }
        public Guid KlasaId { get; set; }
        public Klasa Klasa { get; set; }
        public Guid ObradivostId { get; set; }
        public Obradivost Obradivost { get; set; }
        public Guid ParcelaId { get; set; }
        public Parcela Parcela { get; set; }


        public Guid? KupacId { get; set; }


    }
}
