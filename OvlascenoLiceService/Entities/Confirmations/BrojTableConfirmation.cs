using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OvlascenoLiceService.Entities.Confirmations
{
    /// <summary>
    /// Predstavlja potvrdu kreiranja broja table 
    /// </summary>
    public class BrojTableConfirmation
    {
        /// <summary>
        /// ID broja table
        /// </summary>
        public Guid BrojTableId { get; set; }
        /// <summary>
        /// Oznaka table
        /// </summary>
        public string OznakaTable { get; set; }
    }
}
