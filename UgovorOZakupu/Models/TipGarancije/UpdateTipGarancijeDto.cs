using System;

namespace UgovorOZakupu.Models.TipGarancije
{
    /// <summary>
    /// Model tipa garancije za izmenu
    /// </summary>
    public class UpdateTipGarancijeDto
    {
        /// <summary>
        /// Id tipa garancije
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Naziv tipa garancije
        /// </summary>
        public string NazivTipa { get; set; }
    }
}