using Microsoft.IdentityModel.Tokens; // Dostarcza klasy do pracy z tokenami
using System.IdentityModel.Tokens.Jwt; // Obsługa generowania tokenów JWT
using System.Security.Claims; // Umożliwia dodawanie claimów (informacji o użytkowniku)
using System.Text; // Do konwersji tekstu na bajty

namespace Projekt.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za generowanie tokenów JWT.
    /// </summary>
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Konstruktor przyjmujący IConfiguration do odczytu ustawień JWT z appsettings.json.
        /// </summary>
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generuje token JWT dla podanego użytkownika.
        /// </summary>
        /// <param name="username">Nazwa użytkownika (login lub e-mail)</param>
        /// <returns>Wygenerowany token JWT jako string</returns>
        public string GenerateJwtToken(string username)
        {
            // Definiujemy claimy (informacje zawarte w tokenie). W tym przypadku tylko nazwa użytkownika.
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username) // Claim o nazwie użytkownika
            };

            // Pobieramy klucz szyfrujący z konfiguracji (appsettings.json) i konwertujemy na bajty
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "")
            );

            // Tworzymy poświadczenia podpisu przy użyciu algorytmu HmacSha256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tworzymy token JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "", // Issuer z konfiguracji
                audience: _configuration["Jwt:Audience"] ?? "", // Audience z konfiguracji
                claims: claims, // Lista claimów
                expires: DateTime.Now.AddHours(1), // Token ważny przez 1 godzinę
                signingCredentials: creds // Podpisujemy token naszym kluczem
            );

            // Zwracamy token w formacie string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
