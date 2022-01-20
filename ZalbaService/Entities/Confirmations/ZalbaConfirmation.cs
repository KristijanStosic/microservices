using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities.Confirmations
{
    public class ZalbaConfirmation
    {
        public Guid ZalbaId { get; set; }
        public string BrojNadmetanja { get; set; }
        public string BrojResenja { get; set; }
        public DateTime DatumPodnosenja { get; set; }
        public DateTime DatumResenja { get; set; }
    }
}
