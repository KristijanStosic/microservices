using System;

namespace JavnoNadmetanjeService.Entities.ManyToMany
{
    public class JavnoNadmetanjeDeoParcele
    {
        public Guid JavnoNadmetanjeId { get; set; }
        public JavnoNadmetanje JavnoNadmetanje { get; set; }
        public Guid DeoParceleId { get; set; }
    }
}
