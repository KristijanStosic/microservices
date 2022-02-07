using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OvlascenoLiceService.Entities
{
    /// <summary>
    /// Predstavlja ovlasceno lice
    /// </summary>
    public class OvlascenoLice
    {
        /// <summary>
        /// ID ovlascenog lica
        /// </summary>
        [Key]
        public Guid OvlascenoLiceId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Ime ovlascenog lica
        /// </summary>
        [Required]
        public string Ime { get; set; }
        /// <summary>
        /// Prezime ovlascenog lica
        /// </summary>
        [Required]
        public string Prezime { get; set; }
        /// <summary>
        /// JMBG ovlascenog lica - 13 brojeva
        /// </summary>
        [MaxLength(13)]
        [MinLength(13)]
        public string JMBG { get; set; }
        /// <summary>
        /// Broj pasosa ovlascenog lica
        /// </summary>
        public string BrojPasosa { get; set; }

        /// <summary>
        /// Lista brojeva tabli
        /// </summary>
        public List<BrojTable> BrojeviTabli { get; set; }

        /// <summary>
        /// Id adrese - veza sa mikroservisom Adresa - adresa se ne unosi za strance
        /// </summary>
        public Guid? AdresaId { get; set; }
        /// <summary>
        /// Id drzave - veza sa mikroservisom Adresa - drzava se unosi za strance
        /// </summary>
        public Guid? DrzavaId { get; set; }
    }
}
