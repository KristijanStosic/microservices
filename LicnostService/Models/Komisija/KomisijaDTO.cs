using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LicnostService.Models.Licnost;
using LicnostService.Models.OtherModels;

namespace LicnostService.Models.Komisija
{
    /// <summary>
    /// Model za komisiju
    /// </summary>
    public class KomisijaDto
    {

        /// <summary>
        /// ID komisije
        /// </summary>
        public Guid KomisijaId { get; set; }

        /// <summary>
        /// Naziv komisije
        /// </summary>
        public string NazivKomisije { get; set; }

        /// <summary>
        /// Predsednik komisije
        /// </summary>
        public LicnostDto PredsednikKomisije { get; set; }

        /// <summary>
        /// Lista članova komisije
        /// </summary>

        public List<LicnostDto> ClanoviKomisije { get; set; }

        /// <summary>
        /// Dokument u komisiji
        /// </summary>

        public DokumentDto Dokument{ get; set; }



    }
}
