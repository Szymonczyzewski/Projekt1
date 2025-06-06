// Importujemy wymagane przestrzenie nazw
using Microsoft.AspNetCore.Http;             // Potrzebne do pracy z kontekstem HTTP
using Microsoft.Extensions.Logging;          // Dla logowania błędów
using System;                                // Standardowe typy jak Exception
using System.Net;                            // Zawiera kody statusu HTTP
using System.Text.Json;                      // Do serializacji obiektu do JSON-a
using System.Threading.Tasks;                // Umożliwia użycie async/await

namespace Projekt.Middleware
{
    // Klasa pośrednicząca (middleware), która przechwytuje błędy globalnie
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;                      // Reprezentuje następne ogniwo w pipeline
        private readonly ILogger<ErrorHandlingMiddleware> _logger;   // Logger do zapisu błędów

        // Konstruktor – otrzymuje kolejne ogniwo oraz logger
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Główna metoda wywoływana przy każdym żądaniu HTTP
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Przekazujemy żądanie dalej (do kontrolera lub kolejnego middleware)
                await _next(context);
            }
            catch (Exception ex)
            {
                // Jeśli wystąpił wyjątek, logujemy go do systemu logowania
                _logger.LogError(ex, "Wystąpił błąd podczas przetwarzania żądania.");

                // Ustawiamy typ odpowiedzi i kod statusu (500 - Internal Server Error)
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Tworzymy prosty komunikat błędu w formacie JSON
                var result = JsonSerializer.Serialize(new
                {
                    error = "Wystąpił błąd serwera. Spróbuj ponownie później."
                });

                // Wysyłamy odpowiedź do klienta
                await context.Response.WriteAsync(result);
            }
        }
    }
}
