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
    public class KomisijaDTO
    {

        /// <summary>
        /// ID komisije
        /// </summary>
        /// 
        public Guid KomisijaId { get; set; }

        /// <summary>
        /// Naziv komisije
        /// </summary>
        public string NazivKomisije { get; set; }

        /// <summary>
        /// Predsednik komisije
        /// </summary>
        public LicnostDTO PredsednikKomisije { get; set; }

        /// <summary>
        /// Lista članova komisije
        /// </summary>

        public List<LicnostDTO> ClanoviKomisije { get; set; }

        /// <summary>
        /// Dokument u komisiji
        /// </summary>

        public DokumentDTO Dokument{ get; set; }



    }
}
