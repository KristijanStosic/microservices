using JavnoNadmetanjeService.Models.Etapa;
using System;
using System.Collections.Generic;

namespace JavnoNadmetanjeService.Models.JavnoNadmetanje
{
    /// <summary>
    /// Javno nadmetanje info za pristup od strane drugih servisa
    /// </summary>
    public class JavnoNadmetanjeInfoDto
    {
        /// <summary>
        /// ID javnog nadmetanja
        /// </summary>
        public Guid JavnoNadmetanjeId { get; set; }
        /// <summary>
        /// Pocetna cena po hektaru
        /// </summary>
        public double PocetnaCenaHektar { get; set; }
        /// <summary>
        /// Visina dopune depozita
        /// </summary>
        public int VisinaDopuneDepozita { get; set; }
        /// <summary>
        /// Period zakupa u godinama
        /// </summary>
        public int PeriodZakupa { get; set; }
        /// <summary>
        /// Izlicitirana cena
        /// </summary>
        public int IzlicitiranaCena { get; set; }
        /// <summary>
        /// Broj ucesnika na javnom nadmetanju
        /// </summary>
        public int BrojUcesnika { get; set; }
        /// <summary>
        /// Krug po redu
        /// </summary>
        public int Krug { get; set; }
        /// <summary>
        /// Da li je javno nadmetanje izuzeto
        /// </summary>
        public bool Izuzeto { get; set; }
        /// <summary>
        /// ID statusa javnog nadmetanja
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// ID tipa javnog nadmetanja
        /// </summary>
        public string Tip { get; set; }
        /// <summary>
        /// Lista etapa javnog nadmetanja
        /// </summary>
        public List<EtapaDto> Etape { get; set; }
    }
}
