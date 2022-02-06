using System;

namespace JavnoNadmetanjeService.Models.Status
{
    /// <summary>
    /// Model za status javnog nadmetanja
    /// </summary>
    public class StatusDto
    {
        /// <summary>
        /// Id statusa javnog nadmetanja
        /// </summary>
        public Guid StatusId { get; set; }
        /// <summary>
        /// Naziv statusa javnog nadmetanjaa
        /// </summary>
        public string NazivStatusa { get; set; }
    }
}
