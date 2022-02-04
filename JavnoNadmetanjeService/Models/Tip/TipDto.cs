using System;

namespace JavnoNadmetanjeService.Models.Tip
{
    /// <summary>
    /// Model za tip javnog nadmetanja
    /// </summary>
    public class TipDto
    {
        /// <summary>
        /// Id tipa javnog nadmetanja
        /// </summary>
        public Guid TipId { get; set; }
        /// <summary>
        /// Naziv tipa javnog nadmetanja
        /// </summary>
        public string NazivTipa { get; set; }
    }
}
