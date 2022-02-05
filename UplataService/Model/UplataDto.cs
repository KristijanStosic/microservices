using System;
using UplataService.Entities;

namespace UplataService.Model
{
    /// <summary>
    /// DTO Uplate
    /// </summary>
    public class UplataDto
    {
        /// <summary>
        /// ID uplate
        /// </summary>
        public Guid UplataId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Broj racuna
        /// </summary>
        public string BrojRacuna { get; set; }
        /// <summary>
        /// Poziv na broj
        /// </summary>
        public string PozivNaBroj { get; set; }
        /// <summary>
        /// Iznos 
        /// </summary>
        public double Iznos { get; set; }
        /// <summary>
        /// Svrha uplate
        /// </summary>
        public string SvrhaUplate { get; set; }
        /// <summary>
        /// Datum uplate
        /// </summary>
        public DateTime DatumUplate { get; set; }
        /// <summary>
        /// Id javnog nadmetanja - veza sa mikroservisom Javno nadmetanje
        /// </summary>
        public string JavnoNadmetanje { get; set; }
        /// <summary>
        /// Kurs
        /// </summary>
        public Kurs Kurs { get; set; }
    }
}
