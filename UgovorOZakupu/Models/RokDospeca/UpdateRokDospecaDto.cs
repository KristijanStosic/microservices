using System;

namespace UgovorOZakupu.Models.RokDospeca
{
    /// <summary>
    /// Model roka dospeća za izmenu
    /// </summary>
    public class UpdateRokDospecaDto
    {
        /// <summary>
        /// Id roka dospeća
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Rok dospeća
        /// </summary>
        public int Rok { get; set; }

        /// <summary>
        /// Id ugovora o zakupu
        /// </summary>
        public Guid UgovorOZakupuId { get; set; }
    }
}