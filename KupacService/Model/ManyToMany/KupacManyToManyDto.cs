using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.ManyToMany
{
    /// <summary>
    /// Dto za vraćanje kupaca u vezama više prema više (Ovlašćeno lice i uplata)
    /// </summary>
    public class KupacManyToManyDto
    {
        /// <summary>
        /// Id kupca
        /// </summary>
        public Guid KupacId { get; set; }
    }
}
