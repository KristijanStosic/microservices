using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UplataService.Model
{
    /// <summary>
    /// Vraca potvrdu o uplati
    /// </summary>
    public class UplataConfirmationDto
    {
        /// <summary>
        /// Broj racuna
        /// </summary>
        public string BrojRacuna { get; set; }
        /// <summary>
        /// Poziv na broj
        /// </summary>
        public string PozivNaBroj { get; set; }
        /// <summary>
        /// Datum uplate
        /// </summary>
        public DateTime DatumUplate { get; set; }
        /// <summary>
        /// Iznos uplate
        /// </summary>
        public double Iznos { get; set; }
        /// <summary>
        /// Svrha uplate
        /// </summary>
        public string SvrhaUplate { get; set; }
    }
}
