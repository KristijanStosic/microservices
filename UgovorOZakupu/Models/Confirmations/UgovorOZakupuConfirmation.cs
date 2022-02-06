using System;

namespace UgovorOZakupu.Models.Confirmations
{
    /// <summary>
    ///     Model potvrde ugovora o zakupu
    /// </summary>
    public class UgovorOZakupuConfirmation
    {
        /// <summary>
        ///     Zavodni broj ugovora o zakupu
        /// </summary>
        public string ZavodniBroj { get; set; }

        /// <summary>
        ///     Datum zavođenja ugovora
        /// </summary>
        public DateTime DatumZavodjenja { get; set; }

        /// <summary>
        ///     Rok za vraćanje ugovora
        /// </summary>
        public DateTime RokZaVracanje { get; set; }

        /// <summary>
        ///     Mesto potpisivanja ugovora
        /// </summary>
        public string MestoPotpisivanja { get; set; }

        /// <summary>
        ///     Datum potpisavanja ugovora
        /// </summary>
        public DateTime DatumPotpisivanja { get; set; }
    }
}