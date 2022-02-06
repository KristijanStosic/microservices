using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OvlascenoLiceService.Entities
{
    /// <summary>
    /// Predstavlja broj table 
    /// </summary>
    public class BrojTable
    {
        /// <summary>
        /// ID broja table
        /// </summary>
        [Key]
        public Guid BrojTableId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Redni broj table
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RbTable { get; set; }
        /// <summary>
        /// Oznaka table
        /// </summary>
        [Required]
        public string OznakaTable { get; set; }

        /// <summary>
        /// Id ovlascenog lica - strani kljuc
        /// </summary>
        public Guid OvlascenoLiceId { get; set; }
        /// <summary>
        /// Objekat Ovlasceno lice
        /// </summary>
        public OvlascenoLice OvlascenoLice { get; set; }
    }
}
