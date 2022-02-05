﻿using System;

namespace UgovorOZakupu.Models.UgovorOZakupu
{
    /// <summary>
    /// Model ugovora o zakupu za izmenu
    /// </summary>
    public class UpdateUgovorOZakupuDto
    {
        /// <summary>
        /// Id ugovora o zakupu
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Zavodni broj ugovora o zakupu
        /// </summary>
        public string ZavodniBroj { get; set; }

        /// <summary>
        /// Datum zavođenja ugovora
        /// </summary>
        public DateTime DatumZavodjenja { get; set; }

        /// <summary>
        /// Rok za vraćanje ugovora
        /// </summary>
        public DateTime RokZaVracanje { get; set; }

        /// <summary>
        /// Mesto potpisivanja ugovora
        /// </summary>
        public string MestoPotpisivanja { get; set; }

        /// <summary>
        /// Datum potpisavanja ugovora
        /// </summary>
        public DateTime DatumPotpisivanja { get; set; }

        /// <summary>
        /// Id tipa garancije
        /// </summary>
        public Guid TipGarancijeId { get; set; }
        
        /// <summary>
        /// Id dokumenta (Konačna odluka)
        /// </summary>
        public Guid DokumentId { get; set; }
        
        /// <summary>
        /// Id javnog nadmentanja
        /// </summary>
        public Guid JavnoNadmetanjeId { get; set; }
        
        /// <summary>
        /// Id kupca (Lice)
        /// </summary>
        public Guid KupacId { get; set; }
        
        /// <summary>
        /// Id ličnosti (Ministar)
        /// </summary>
        public Guid LicnostId { get; set; }
    }
}