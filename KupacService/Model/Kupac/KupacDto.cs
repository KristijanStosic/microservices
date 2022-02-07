using KupacService.Model.KontaktOsoba;
using KupacService.Model.OtherServices;
using KupacService.Model.Prioritet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac
{
    /// <summary>
    /// Dto za kupca
    /// </summary>
    public class KupacDto
    {
        /// <summary>
        /// Id kupca
        /// </summary>
        public Guid KupacId { get; set; }
        /// <summary>
        /// Naziv kupca
        /// </summary>
        public string Naziv { get; set; }
        /// <summary>
        /// Broj faksa kupca
        /// </summary>
        public string Faks { get; set; }
        /// <summary>
        /// Ostvarena površina kupca u licitacijama
        /// </summary>
        public double OstvarenaPovrsina { get; set; }
        /// <summary>
        /// Oznaka da li kupac ima zabranu za učestvovanje u licitacijama
        /// </summary>
        public bool ImaZabranu { get; set; }
        /// <summary>
        /// Datum početka zabrane
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime DatumPocetkaZabrane { get; set; }
        /// <summary>
        /// Dužina trajanja zabrane u godinama
        /// </summary>
        public int DuzinaTrajanjaZabraneGod { get; set; }
        /// <summary>
        /// Broj telefona kupca
        /// </summary>
        public string BrojTelefona { get; set; }
        /// <summary>
        /// Drugi broj telefona kupca
        /// </summary>
        public string BrojTelefona2 { get; set; }
        /// <summary>
        /// Emali kupca
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Broj računa kupca
        /// </summary>
        public string BrojRacuna { get; set; }
        /// <summary>
        /// Kontakt osoba kupca
        /// </summary>
        public KontaktOsobaDto KontaktOsoba { get; set; }
        /// <summary>
        /// Prioriteti kupca
        /// </summary>
        public List<PrioritetDto> Prioriteti { get; set; }
        /// <summary>
        /// Adresa kupca
        /// </summary>
        public AdresaDto Adresa { get; set; }
        /// <summary>
        /// Ovlašćena lica koja sarađuju sa kupcem
        /// </summary>
        public List<OvlascenoLiceDto> OvlascenaLica { get; set; }
        /// <summary>
        ///  Uplate koje je izvršio kupac
        /// </summary>
        public List<UplataDto> Uplate { get; set; }
    }
}
