using KorisnikSistemaService.Auth;
using KorisnikSistemaService.Data.Interfaces;
using KorisnikSistemaService.Entities;
using KorisnikSistemaService.Models.Auth;
using KorisnikSistemaService.ServiceCalls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KorisnikSistemaService.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class AuthController : ControllerBase
    {
        private readonly IKorisnikSistemaRepository _korisnikSistemaRepository;
        private readonly ILoggerService _loggerService;
        private readonly IJwtAuthManager _jwtAuthManager;
        public AuthController(IKorisnikSistemaRepository korisnikSistemaRepository, ILoggerService loggerService, IJwtAuthManager jwtAuthManager)
        {
            _korisnikSistemaRepository = korisnikSistemaRepository;
            _loggerService = loggerService;
            _jwtAuthManager = jwtAuthManager;
        }

        /// <summary>
        /// Autentifikacija
        /// </summary>
        /// <param name="authCreds"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthCreds authCreds)
        {
            KorisnikSistema korisnikSistema = await _korisnikSistemaRepository.GetKorisnikSistemaByKorisnickoIme(authCreds.KorisnickoIme);

            if (korisnikSistema == null)
            {
                await _loggerService.Log(LogLevel.Warning, "Authenticate", "Korisnik sa ovim korisnickim imenom ne postoji");
                return NotFound();
            }else if (!BCrypt.Net.BCrypt.Verify(authCreds.Lozinka, korisnikSistema.Lozinka))
            {
                await _loggerService.Log(LogLevel.Warning, "Authenticate", "Unesena naispravna lozinka");
                return Unauthorized();
            }
            string nazivTipaKorisnika = korisnikSistema.TipKorisnika.NazivTipaKorisnika;

            JwtToken token = _jwtAuthManager.Authenticate(korisnikSistema.KorisnickoIme, korisnikSistema.Lozinka, nazivTipaKorisnika);
            if(token == null)
            {
                await _loggerService.Log(LogLevel.Warning, "Authenticate", "Token nije generisan!");
                return Unauthorized();
            }
            
            return Ok(token);
        }

    }
}
