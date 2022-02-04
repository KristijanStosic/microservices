using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LicnostService.Models.Licnost;

namespace LicnostService.Models.Komisija
{
    /// <summary>
    /// Model za izmenu komisije
    /// </summary>
    public class KomisijaUpdateDTO
    {
        /// <summary>
        /// ID komisije
        /// </summary>
        public Guid KomisijaId { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti naziv komisije.")]

        /// <summary>
        /// Naziv komisije
        /// </summary>
        public string NazivKomisije { get; set; }

        /// <summary>
        /// ID predsednika komisije
        /// </summary>

        public Guid PredsednikKomisijeId { get; set; }

        /// <summary>
        /// Lista clanova komisije
        /// </summary>

        public List<Guid> ClanoviKomisije { get; set; }

        /// <summary>
        /// ID dokumenta
        /// </summary>

        public Guid? DokumentId { get; set; }



    }
}

