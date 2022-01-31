using System;

namespace UgovorOZakupu.Models.RokDospeca
{
    public class CreateRokDospecaDto
    {
        public int Rok { get; set; }

        public Guid UgovorOZakupuId { get; set; }
    }
}