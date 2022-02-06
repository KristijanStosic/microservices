using System;

namespace PrijavaService.Entities.ManyToMany
{
    public class PrijavaJavnoNadmetanje
    {
        public Guid PrijavaId { get; set; }
        public Prijava Prijava { get; set; }
        public Guid JavnoNadmetanjeId { get; set; }
    }
}
