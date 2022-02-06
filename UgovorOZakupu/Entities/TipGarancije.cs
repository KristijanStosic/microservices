using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UgovorOZakupu.Entities
{
    [Table("TipGarancije")]
    public class TipGarancije
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string NazivTipa { get; set; }
    }
}