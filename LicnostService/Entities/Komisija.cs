using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Entities
{
    public class Komisija
    {
        [Key]
        public Guid KomisijaId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivKomisije { get; set; }

        public Guid PredsednikKomisijeId { get; set; }


        public Licnost PredsednikKomisije { get; set; }

        public List<Licnost> ClanoviKomisije { get; set; }

        
        public Guid? DokumentId { get; set; }
    }
}
