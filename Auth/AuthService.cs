// Importujemy potrzebne przestrzenie nazw do tworzenia i podpisywania tokenów JWT
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projekt.Auth
{
    // Klasa odpowiedzialna za generowanie tokenów JWT
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        // Konstruktor przyjmuje konfigurację aplikacji (np. dane z appsettings.json)
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Metoda generuje token JWT dla podanego użytkownika
        public string GenerateJwtToken(string username)
        {
            // Pobranie ustawień JWT z pliku konfiguracyjnego (appsettings.json)
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            // Sprawdzamy, czy nie brakuje którejś wartości w konfiguracji
            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("Brakuje konfiguracji JWT w appsettings.json (Jwt:Key, Jwt:Issuer, Jwt:Audience)");
            }

            // Ustalanie roli użytkownika – jeśli login to 'admin', przypisujemy rolę Admin, w przeciwnym razie User
            var role = username.ToLower() == "admin" ? "Admin" : "User";

            // Tworzymy listę "roszczeń" (claims) – są to informacje zakodowane w tokenie
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),   // Nazwa użytkownika
                new Claim(ClaimTypes.Role, role)        // Rola (Admin lub User)
            };

            // Tworzymy klucz bezpieczeństwa na podstawie naszego sekretu (Key)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            // Tworzymy dane do podpisu tokena przy użyciu algorytmu HMAC SHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tworzymy obiekt tokena JWT, ustalając:
            // - wydawcę (issuer)
            // - odbiorcę (audience)
            // - roszczenia (claims)
            // - datę wygaśnięcia (ważność przez 2 godziny)
            // - dane do podpisu
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            // Serializujemy token do postaci tekstowej (string) i go zwracamy
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
