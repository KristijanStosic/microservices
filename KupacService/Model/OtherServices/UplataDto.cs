using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.OtherServices
{
    public class UplataDto
    {
        public string BrojRacuna { get; set; }
        public string PozivNaBroj { get; set; }
        public string SvrhaUplate { get; set; }
        public double Iznos { get; set; }
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
    }
}
