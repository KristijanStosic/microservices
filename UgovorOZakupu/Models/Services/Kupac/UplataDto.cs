using System;

namespace UgovorOZakupu.Models.Kupac
{
    public class UplataDto
    {
        public string BrojRacuna { get; set; }
        public string PozivNaBroj { get; set; }
        public string SvrhaUplate { get; set; }
        public double Iznos { get; set; }
        public DateTime Datum { get; set; }
    }
}