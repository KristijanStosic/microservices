using System;

namespace RabbitMQ.Consumer.Models
{
    public class MailInfo
    {
        public string Email { get; set; }
        public string Kupac { get; set; }
        public string Adresa { get; set; }
        public DateTime DatumOdrzavanja { get; set; }
        public string VremePocetka { get; set; }
        public string VremeKraja { get; set; }
    }
}
