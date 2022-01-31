using System;

namespace DocumentService.Models.TipDokumenta
{
    public class UpdateTipDokumentaDto
    { 
        public Guid Id { get; set; }
        
        public string NazivTipa { get; set; }
    }
}