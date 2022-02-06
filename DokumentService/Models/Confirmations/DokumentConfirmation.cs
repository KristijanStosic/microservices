using System;

namespace DokumentService.Models.Confirmations
{
    /// <summary>
    ///     Model potvrde dokumenta
    /// </summary>
    public class DokumentConfirmation
    {
        /// <summary>
        ///     Zavodni broj dokumenta
        /// </summary>
        public string ZavodniBroj { get; set; }

        /// <summary>
        ///     Datum zavođenja dokumenta
        /// </summary>
        public DateTime Datum { get; set; }
    }
}