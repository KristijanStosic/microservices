using System;

namespace JavnoNadmetanjeService.Entities.ManyToMany
{
    public class JavnoNadmetanjeKupac
    {
        public Guid JavnoNadmetanjeId { get; set; }
        public JavnoNadmetanje JavnoNadmetanje { get; set; }
        public Guid KupacId { get; set; }
    }
}
