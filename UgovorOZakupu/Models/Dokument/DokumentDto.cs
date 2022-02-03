using System;

namespace UgovorOZakupu.Models.Dokument
{
    public class DokumentDto
    {
        public string ZavodniBroj { get; set; }
        
        public DateTime Datum { get; set; }
        
        public DateTime DatumDonosenjaDokumenta { get; set; }
        
        public string TipDokumenta { get; set; }
    }
}