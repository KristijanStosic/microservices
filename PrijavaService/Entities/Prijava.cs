using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrijavaService.Entities
{
    public class Prijava
    {
        [Key]
        public Guid PrijavaId { get; set; } = Guid.NewGuid();
        [Required]
        public string BrojPrijave { get; set; }
        [Required]
        public DateTime DatumPrijave { get; set; }
        public string MestoPrijave { get; set; }
        public string SatPrijema { get; set; }
        public bool ZatvorenaPonuda { get; set; }

        public List<DokFizickaLica> DokFizickaLica { get; set; }
        public List<DokPravnaLica> DokPravnaLica { get; set; }

        [NotMapped]
        public List<Guid> JavnoNadmetanje { get; set; }

    }
}
