
using KupacService.Model.OtherServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac
{/// <summary>
/// Pomoćni Dto za program
/// </summary>
    public class KupacOtherServicesDto
    {

        public AdresaDto Adresa { get; set; }
        public List<OvlascenoLiceDto> OvlascenaLica { get;set;}
        public List<UplataDto> Uplate { get; set; }


    }
}
