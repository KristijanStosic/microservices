using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class Parcela
    {
        [Key]
        public Guid ParcelaId { get; set; } = Guid.NewGuid();

        [Required]
        public string BrojParcele { get; set; }
        [Required]
        public string PovrsinaParcele { get; set; }
        public string BrojListeNepokretnosti { get; set; }

        public Guid ZasticenaZonaId { get; set; }
        public ZasticenaZona ZasticenaZona { get; set; }
        public Guid OblikSvojineId { get; set; }
        public OblikSvojine OblikSvojine { get; set; }
        public Guid OdvodnjavanjeId { get; set; }
        public Odvodnjavanje Odvodnjavanje { get; set; }
        public Guid KatastarskaOpstinaId { get; set; }
        public KatastarskaOpstina KatastarskaOpstina { get; set; }

        public List<DeoParcele> DeloviParcele { get; set; }
    
    }
}
