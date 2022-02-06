using System;

namespace UgovorOZakupu.Models.Kupac
{
    /// <summary>
    ///     Dto za adresu
    /// </summary>
    public class AdresaDto
    {
        /// <summary>
        ///     Id adrese
        /// </summary>
        public Guid AdresaId { get; set; }

        /// <summary>
        ///     Naziv ulice
        /// </summary>
        public string Ulica { get; set; }

        /// <summary>
        ///     Broj adrese
        /// </summary>
        public string Broj { get; set; }

        /// <summary>
        ///     Mesto u kom se adresa nalazi
        /// </summary>
        public string Mesto { get; set; }

        /// <summary>
        ///     Poštanski broj
        /// </summary>
        public string PostanskiBroj { get; set; }

        /// <summary>
        ///     Naziv države
        /// </summary>
        public string Drzava { get; set; }
    }
}