namespace JavnoNadmetanjeService.Models.JavnoNadmetanje
{
    /// <summary>
    /// Model za potvrdu kreiranja javnog nadmetanja
    /// </summary>
    public class JavnoNadmetanjeConfirmationDto
    {
        /// <summary>
        /// Pocetna cena po hektaru
        /// </summary>
        public double PocetnaCenaHektar { get; set; }
        /// <summary>
        /// Period zakupa u godinama
        /// </summary>
        public int PeriodZakupa { get; set; }
        /// <summary>
        /// Krug po redu
        /// </summary>
        public int Krug { get; set; }
        /// <summary>
        /// Visina dopuna depozita
        /// </summary>
        public int VisinaDopuneDepozita { get; set; }
    }
}
