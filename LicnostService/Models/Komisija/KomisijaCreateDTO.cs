using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LicnostService.Models.Licnost;

namespace LicnostService.Models.Komisija
{
    /// <summary>
    /// Model za kreiranje komisije
    /// </summary>
    public class KomisijaCreateDto
    {
        /// <summary>
        /// Naziv komisije
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv komisije.")]
        public string NazivKomisije { get; set; }
        /// <summary>
        /// ID predsednika komisije
        /// </summary>
        public Guid PredsednikKomisijeId { get; set; }
        /// <summary>
        /// Lista članova komisije
        /// </summary>
        public List<Guid> ClanoviKomisije { get; set; }

        /// <summary>
        /// ID dokumenta u okviru komisije
        /// </summary>
        public Guid? DokumentId { get; set; }

    }
}

        