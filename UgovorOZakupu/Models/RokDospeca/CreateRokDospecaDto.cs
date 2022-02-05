using System;

namespace UgovorOZakupu.Models.RokDospeca
{
    /// <summary>
    /// Model roka dospeća za kreiranje
    /// </summary>
    public class CreateRokDospecaDto
    {
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