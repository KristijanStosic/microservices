namespace KorisnikSistemaService.Auth
{
    public interface IJwtAuthManager
    {
        JwtToken Authenticate(string korisnickoIme, string lozinka, string tipKorisnika);
    }
}
