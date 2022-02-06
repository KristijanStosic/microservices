using KupacService.Model.OtherServices;
using KupacService.Model.Prioritet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace KupacService.Model.Kupac.FizickoLice
{
    /// <summary>
    /// Fizičko lice dto
    /// </summary>
    public class FizickoLiceDto
    {
        /// <summary>
        /// Id fizičkog lica
        /// </summary>
        public Guid KupacId { get; set; }
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
        public List<PrioritetDto> Prioriteti { get; set; }
        /// <summary>
        /// Adresa fizičkog lica
        /// </summary>
        public AdresaDto AdresaId { get; set; }
        /// <summary>
        /// Ovlašćena lica koja sarađuju sa fizičkim licem
        /// </summary>
        public List<OvlascenoLiceDto> OvlascenaLica { get; set; }
        /// <summary>
        /// Uplate fizičkog lica
        /// </summary>
        public List<UplataDto> Uplate { get; set; }


    }
}
