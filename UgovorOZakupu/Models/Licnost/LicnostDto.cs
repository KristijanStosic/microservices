using System;

namespace UgovorOZakupu.Models.Licnost
{
    public class LicnostDto
    {
        public Guid LicnostId { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string Funkcija { get; set; }
    }
}