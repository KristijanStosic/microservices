using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.OtherServices
{
    /// <summary>
    /// Dto za uplatu
    /// </summary>
    public class UplataDto
    {
        /// <summary>
        /// Broj računa na koji se vrši uplata
        /// </summary>
        public string BrojRacuna { get; set; }
        /// <summary>
        /// Poziv na broj
        /// </summary>
        public string PozivNaBroj { get; set; }
        /// <summary>
        /// Svrha uplate
        /// </summary>
        public string SvrhaUplate { get; set; }
        /// <summary>
        /// Iznos uplate
        /// </summary>
        public double Iznos { get; set; }
        /// <summary>
        /// Datum uplate
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
    }
}
