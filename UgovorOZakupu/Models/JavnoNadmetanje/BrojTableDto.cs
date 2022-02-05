using System;

namespace UgovorOZakupu.Models.JavnoNadmetanje
{
    /// <summary>
    /// Model za broj table
    /// </summary>
    public class BrojTableDto
    {
        /// <summary>
        /// ID broja table
        /// </summary>
        public Guid BrojTableId { get; set; }

        /// <summary>
        /// Redni broj table
        /// </summary>
        public int RbTable { get; set; }

        /// <summary>
        /// Oznaka table
        /// </summary>
        public string OznakaTable { get; set; }
    }
}