using System;
using System.Collections.Generic;

namespace UgovorOZakupu.Models.JavnoNadmetanje
{
    /// <summary>
    ///     Model javnog nadmetanja
    /// </summary>
    public class JavnoNadmetanjeDto
    {
        /// <summary>
        ///     ID javnog nadmetanja
        /// </summary>
        public Guid JavnoNadmetanjeId { get; set; }

        /// <summary>
        ///     Pocetna cena po hektaru
        /// </summary>
        public double PocetnaCenaHektar { get; set; }

        /// <summary>
        ///     Visina dopune depozita
        /// </summary>
        public int VisinaDopuneDepozita { get; set; }

        /// <summary>
        ///     Period zakupa u godinama
        /// </summary>
        public int PeriodZakupa { get; set; }

        /// <summary>
        ///     Izlicitirana cena
        /// </summary>
        public int IzlicitiranaCena { get; set; }

        /// <summary>
        ///     Broj ucesnika na javnom nadmetanju
        /// </summary>
        public int BrojUcesnika { get; set; }

        /// <summary>
        ///     Krug po redu
        /// </summary>
        public int Krug { get; set; }

        /// <summary>
        ///     Da li je javno nadmetanje izuzeto
        /// </summary>
        public bool Izuzeto { get; set; }

        /// <summary>
        ///     Status javnog nadmetanja
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///     Tip javnog nadmetanja
        /// </summary>
        public string Tip { get; set; }

        /// <summary>
        ///     Lista etapa javnog nadmetanja
        /// </summary>
        public List<EtapaDto> Etape { get; set; }

        /// <summary>
        ///     Adresa javnog nadmetanja
        /// </summary>
        public string Adresa { get; set; }

        /// <summary>
        ///     Najbolji kupac u javnom nadmetanju
        /// </summary>
        public JavnoNadmetanjeKupacDto Kupac { get; set; }

        /// <summary>
        ///     Lista ovlascenih lica
        /// </summary>
        public List<OvlascenoLiceDto> OvlascenaLica { get; set; }

        /// <summary>
        ///     Lista kupaca
        /// </summary>
        public List<JavnoNadmetanjeKupacDto> Kupci { get; set; }

        /// <summary>
        ///     Lista delova parcele
        /// </summary>
        public List<DeoParceleDto> DeloviParcele { get; set; }
    }
}