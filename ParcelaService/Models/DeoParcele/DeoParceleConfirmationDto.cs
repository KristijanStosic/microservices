using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.DeoParcele
{
    public class DeoParceleConfirmationDto
    {
        /// <summary>
        /// Redni broj dela parcele
        /// </summary>
        public string RedniBrojDela { get; set; }
        /// <summary>
        /// Povrsina dela parcele
        /// </summary>
        public string PovrsinaDela { get; set; }
    }
}
