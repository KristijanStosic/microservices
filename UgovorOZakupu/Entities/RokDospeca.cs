using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UgovorOZakupu.Entities
{
    [Table("RokDospeca")]
    public class RokDospeca
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Rok { get; set; }

        public Guid UgovorOZakupuId { get; set; }
        public UgovorOZakupu UgovorOZakupu { get; set; }
    }
}