using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace KorisnikSistemaService.Auth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly string _key;
        public JwtAuthManager(string key)
        {
            this._key = key;
        }

        public JwtToken Authenticate(string korisnickoIme, string lozinka, string tipKorisnika)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, korisnickoIme),
                    new Claim(ClaimTypes.Role, tipKorisnika)
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            JwtToken tokenRes = new JwtToken
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresOn = String.Format("{0:dd-MM-yyyy hh:mm:ss}", (DateTime)tokenDescriptor.Expires)
        };
            return tokenRes;

        }
    }
}
