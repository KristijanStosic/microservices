using System;

namespace KorisnikSistemaService.Auth
{
    public class JwtToken
    {
        public string Token { get; set; }
        public string ExpiresOn { get; set; }
    }
}
