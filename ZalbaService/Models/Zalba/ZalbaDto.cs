using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Models.Services;
using ZalbaService.Models.StatusZalbe;

namespace ZalbaService.Models.Zalba
{
    public class ZalbaDto
    {
        public DateTime DatumPodnosenja { get; set; }

        public DateTime DatumResenja { get; set; }

        public string RazlogZalbe { get; set; }

        public string Obrazlozenje { get; set; }

        public string BrojNadmetanja { get; set; }

        public string BrojResenja { get; set; }

        public string StatusZalbe { get; set; }
        public string TipZalbe { get; set; }
        public string RadnjaZaZalbu { get; set; }

        public string Kupac { get; set; }
    }
}
