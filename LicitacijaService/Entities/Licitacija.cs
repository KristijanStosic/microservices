using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Entities
{
    public class Licitacija
    {
        [Key]
        public Guid LicitacijaId { get; set; } = Guid.NewGuid();
        [Required]
        public int BrojLicitacije { get; set; }
        [Required]
        public int GodinaLicitacije { get; set; }
        public int OgranicenjeLicitacije { get; set; }
        public DateTime RokLicitacije { get; set; }
        [Required]
        public int KorakCeneLicitacije { get; set; }

        [NotMapped]
        public List<Guid> JavnaNadmetanja { get; set; }




    }
}
