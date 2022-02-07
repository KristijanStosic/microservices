using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac.PravnoLice
{
    /// <summary>
    /// Dto za ažuriranje pravnog lica
    /// </summary>
    public class PravnoLiceUpdateDto : IValidatableObject
    {
        /// <summary>
        /// Id pravnog lica
        /// </summary>
        [Required]
        public Guid KupacId { get; set; }
        /// <summary>
        /// naziv pravnog lica
        /// </summary>
        public string Naziv { get; set; }
        /// <summary>
        /// Matični broj pravnog lica
        /// </summary>
        public string MaticniBroj { get; set; }
        /// <summary>
        /// Broj faksa pravnog lica
        /// </summary>
        public string Faks { get; set; }
        /// <summary>
        /// Ostvarena površina u lcitacijama u kojim je učestvovalo pravno lice
        /// </summary>
        public double OstvarenaPovrsina { get; set; }
        /// <summary>
        /// Oznaka da li pravno lice ima zabranu za učestvovanje u licitacijama
        /// </summary>
        public bool ImaZabranu { get; set; }
        /// <summary>
        /// Datum početka zabrane
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime DatumPocetkaZabrane { get; set; }
        /// <summary>
        /// Trajanje zabrane u godinama
        /// </summary>
        public int DuzinaTrajanjaZabraneGod { get; set; }
        /// <summary>
        /// Broj telefona pravnog lica
        /// </summary>
        public string BrojTelefona { get; set; }
        /// <summary>
        /// Drugi broj telefona pravnog lica
        /// </summary>
        public string BrojTelefona2 { get; set; }
        /// <summary>
        /// Email pravnog lica
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Broj računa pravnog lica
        /// </summary>
        public string BrojRacuna { get; set; }
        /// <summary>
        /// Kontakt osoba pravnog lica
        /// </summary>
        [Required]
        public Guid KontaktOsobaId { get; set; }
        /// <summary>
        /// Prioriteti pravnog lica
        /// </summary>
        public List<Guid> Prioriteti { get; set; }
        /// <summary>
        /// Adresa pravnog lica
        /// </summary>
        public Guid AdresaId { get; set; }
        /// <summary>
        /// Ovlašćena lica koja sarađuju sa pravnim licem
        /// </summary>
        public List<Guid> OvlascenaLica { get; set; }
        /// <summary>
        /// Izvršene uplate pravnog lica
        /// </summary>
        public List<Guid> Uplate { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OstvarenaPovrsina < 0)
            {
                yield return new ValidationResult(
                     "Ostvarena površina ne može biti manja od 0",
                     new[] { "PravnoLiceUpdateDto" });
            }
        }
    }
}
