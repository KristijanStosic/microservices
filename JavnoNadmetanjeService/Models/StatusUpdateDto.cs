using System;

namespace JavnoNadmetanjeService.Models
{
    public class StatusUpdateDto
    {
        public Guid StatusId { get; set; }

        public string NazivStatusa { get; set; }
    }
}
