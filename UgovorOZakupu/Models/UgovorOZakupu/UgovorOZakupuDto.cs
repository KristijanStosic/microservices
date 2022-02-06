using System;
using System.Collections.Generic;
using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;
using UgovorOZakupu.Models.Kupac;
using UgovorOZakupu.Models.Licnost;

namespace UgovorOZakupu.Models.UgovorOZakupu
{
    /// <summary>
    ///     Model ugovora o zakupu
    /// </summary>
    public class UgovorOZakupuDto
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

        /// <summary>
        ///     Tip garancije
        /// </summary>
        public string TipGarancije { get; set; }

        /// <summary>
        ///     Rokovi dospeća
        /// </summary>
        public IEnumerable<int> RokoviDospeca { get; set; }

        /// <summary>
        ///     Konačna odluka (Dokument)
        /// </summary>
        public DokumentDto Odluka { get; set; }

        /// <summary>
        ///     Javno nadmentanje
        /// </summary>
        public JavnoNadmetanjeDto JavnoNadmetanje { get; set; }

        /// <summary>
        ///     Lice
        /// </summary>
        public KupacDto Lice { get; set; }

        /// <summary>
        ///     Ministar
        /// </summary>
        public LicnostDto Ministar { get; set; }
    }
}