using System;

namespace UgovorOZakupu.Models.RokDospeca
{
    public class UpdateRokDospecaDto
    {
        public Guid Id { get; set; }

        public int Rok { get; set; }

        public Guid UgovorOZakupuId { get; set; }
    }
}