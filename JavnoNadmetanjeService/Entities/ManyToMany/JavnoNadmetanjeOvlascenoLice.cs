using System;

namespace JavnoNadmetanjeService.Entities.ManyToMany
{
    public class JavnoNadmetanjeOvlascenoLice
    {
        public Guid JavnoNadmetanjeId { get; set; }
        public JavnoNadmetanje JavnoNadmetanje { get; set; }
        public Guid OvlascenoLiceId { get; set; }
    }
}
