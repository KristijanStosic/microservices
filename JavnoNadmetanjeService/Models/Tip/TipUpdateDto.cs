using System;

namespace JavnoNadmetanjeService.Models.Tip
{
    public class TipUpdateDto
    {
        public Guid TipId { get; set; }
        public string NazivTipa { get; set; }
    }
}
