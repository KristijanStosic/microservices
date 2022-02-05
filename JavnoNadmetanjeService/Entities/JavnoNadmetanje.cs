using JavnoNadmetanjeService.Entities.ManyToMany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavnoNadmetanjeService.Entities
{
    public class JavnoNadmetanje
    {
        [Key]
        public Guid JavnoNadmetanjeId { get; set; } = Guid.NewGuid();
        [Required]
        public double PocetnaCenaHektar { get; set; }
        public int VisinaDopuneDepozita { get; set; }
        [Required]
        public int PeriodZakupa { get; set; }
        public int IzlicitiranaCena { get; set; }
        public int BrojUcesnika { get; set; }
        public int Krug { get; set; }
        public bool Izuzeto { get; set; }

        public Guid StatusId { get; set; }
        public Status Status { get; set; }
        public Guid TipId { get; set; }
        public Tip Tip { get; set; }
        public List<Etapa> Etape { get; set; }

        public Guid? KupacId { get; set; }
        public Guid? AdresaId { get; set; }

        [NotMapped]
        public List<Guid> OvlascenaLica { get; set; }
        [NotMapped]
        public List<Guid> Kupci { get; set; }
        [NotMapped]
        public List<Guid> DeloviParcele { get; set; }

    }
}
