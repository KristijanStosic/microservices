using System;

namespace PrijavaService.Models.DokPravnoLice
{
    public class DokPravnaLicaDto
    {
        /// <summary>
        /// ID dokumenta fizickog lica
        /// </summary>
        public Guid DokPravnaLicaId { get; set; }
        /// <summary>
        /// Naziv dokumenta fizickog lica
        /// </summary>
        public string NazivDokumenta { get; set; }
    }
}
