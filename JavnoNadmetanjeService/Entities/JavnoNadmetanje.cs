using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Entities
{
    public class JavnoNadmetanje
    {
        [Key]
        public Guid JavnoNadmetanjeId { get; set; } = Guid.NewGuid();
        [Required]
        public double PocetnaCenaHektar { get; set; }
        [Required]
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

        //ToDo: Nije dodato nista iz drugih mikroservisa za sada
    }
}
