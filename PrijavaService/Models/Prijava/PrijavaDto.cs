using PrijavaService.Entities;
using PrijavaService.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.Prijava
{
    /// <summary>
    /// Model prijave
    /// </summary>
    public class PrijavaDto
    {
        public Guid PrijavaId { get; set; }
        /// <summary>
        /// Registarski broj prijave
        /// </summary>
        public string BrojPrijave { get; set; }
        /// <summary>
        /// Datum podnosenja prijave
        /// </summary>
        public DateTime DatumPrijave { get; set; }
        /// <summary>
        /// Mjesto na kojem je podnjeta prijava
        /// </summary>
        public string MestoPrijave { get; set; }
        /// <summary>
        /// Sat prijema prijve
        /// </summary>
        public string SatPrijema { get; set; }
        /// <summary>
        /// Oznaka da li je prijava zatvorena
        /// </summary>
        public bool ZatvorenaPonuda { get; set; }

        /// <summary>
        /// Kupac koji je vlasnik prijave
        /// </summary>
        public KupacDto Kupac { get; set; }

        /// <summary>
        /// Lista dokumenata fizickih lica
        /// </summary>
        public List<DokFizickaLica> DokFizickaLica { get; set; }
        /// <summary>
        /// Lista dokumenta pravnih lica
        /// </summary>
        public List<DokPravnaLica> DokPravnaLica { get; set; }

        /// <summary>
        /// Lista javnih nadmetanja za prijavu
        /// </summary>
        public List<JavnoNadmetanjeDto> JavnoNadmetanje { get; set; }

    }
}
