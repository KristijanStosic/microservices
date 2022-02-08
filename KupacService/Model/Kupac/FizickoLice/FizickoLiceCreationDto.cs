using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac.FizickoLice
{
    /// <summary>
    /// Dto za kreiranje fizičkog lica
    /// </summary>
    public class FizickoLiceCreationDto : IValidatableObject
    {
        /// <summary>
        /// Ime fizičkog lica
        /// </summary>
        public string Ime { get; set; }
        /// <summary>
        /// Prezime fizičkog lica
        /// </summary>
        public string Prezime { get; set; }
        /// <summary>
        /// JMBG fizičkog lica
        /// </summary>
        [MinLength(13)]
        [MaxLength(13)]
        public string JMBG { get; set; }
        /// <summary>
        /// Ostvarena površina pri licitaciji od fizičkog lica
        /// </summary>
        public double OstvarenaPovrsina { get; set; }
        /// <summary>
        /// Oznaka da li fizičko lice ima zabranu učestvovanja licitaciji
        /// </summary>
        public bool ImaZabranu { get; set; }
        /// <summary>
        /// Datum početka zabrane
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime DatumPocetkaZabrane { get; set; }
        /// <summary>
        /// Dužina trajanja zabrane
        /// </summary>
        public int DuzinaTrajanjaZabraneGod { get; set; }
        /// <summary>
        /// Broj telefona fizičog lica 
        /// </summary>
        public string BrojTelefona { get; set; }
        /// <summary>
        /// Drugi broj telefona fizičkog lica
        /// </summary>
        public string BrojTelefona2 { get; set; }
        /// <summary>
        /// Email fizičkog lica
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Broj računa fizičkog lica
        /// </summary>
        public string BrojRacuna { get; set; }
        /// <summary>
        /// Pririteti fizičkog lica
        /// </summary>
        public List<Guid> Prioriteti { get; set; }
        /// <summary>
        /// Adresa fizičkog lica
        /// </summary>
        public Guid AdresaId { get; set; }
        /// <summary>
        /// Ovlašćena lica koja sarađuju sa fizičkim licem
        /// </summary>
        public List<Guid> OvlascenaLica { get; set; }
        /// <summary>
        /// Uplate fizičkog lica
        /// </summary>
        public List<Guid> Uplate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OstvarenaPovrsina < 0)
            {
                yield return new ValidationResult(
                     "Ostvarena površina ne može biti manja od 0",
                     new[] { "FizickoLiceCreationDto" });
            }
        }
    }
}
