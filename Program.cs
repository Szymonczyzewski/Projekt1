using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Projekt;
using Projekt.Middleware;
using Projekt.Services;
using System.Text;

// Aliasy dla dwóch różnych AuthService, aby uniknąć konfliktu nazw
using AuthServiceFromAuth = Projekt.Auth.AuthService;
using AuthServiceFromServices = Projekt.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);

// Dodajemy obsługę kontrolerów (API REST)
builder.Services.AddControllers();

// Dodajemy wsparcie dla automatycznego generowania opisów endpointów (używane przez Swagger)
builder.Services.AddEndpointsApiExplorer();

// Konfiguracja Swaggera (OpenAPI) - dokumentacja API i interaktywny interfejs testowania
builder.Services.AddSwaggerGen(c =>
{
    // Definicja dokumentu Swagger z tytułem i wersją API
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Projekt API", Version = "v1" });

    // Dodajemy definicję zabezpieczeń JWT w Swaggerze, aby umożliwić uwierzytelnianie tokenem
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Wprowadź 'Bearer' + spację, a następnie token JWT",
        Name = "Authorization",               // Nazwa nagłówka HTTP, w którym przesyłany jest token
        In = ParameterLocation.Header,        // Token znajduje się w nagłówku HTTP
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Wymaganie, aby wszystkie endpointy korzystały z zabezpieczenia Bearer JWT
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()  // brak konkretnych scope'ów wymaganych
        }
    });
});

// Rejestracja implementacji usługi uwierzytelniania AuthService (wybrana wersja)
builder.Services.AddSingleton<AuthServiceFromAuth>();

// Pobieramy konfigurację JWT z appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// Walidacja, czy wszystkie niezbędne wartości są ustawione
if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("Brakuje konfiguracji JWT w appsettings.json (Jwt:Key, Jwt:Issuer, Jwt:Audience)");
}

// Konfiguracja uwierzytelniania JWT
builder.Services.AddAuthentication(options =>
{
    // Ustawiamy schemat uwierzytelniania JWT jako domyślny
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Parametry weryfikacji tokenu JWT
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,                            // Weryfikuj wystawcę tokenu
        ValidateAudience = true,                          // Weryfikuj odbiorcę tokenu
        ValidateLifetime = true,                          // Weryfikuj czas ważności tokenu
        ValidateIssuerSigningKey = true,                  // Weryfikuj podpis tokenu
        ValidIssuer = jwtIssuer,                          // Oczekiwany wystawca tokenu
        ValidAudience = jwtAudience,                      // Oczekiwany odbiorca tokenu
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))  // Klucz do weryfikacji podpisu
    };
});

// Rejestracja HttpClient dla NbpService z bazowym adresem API NBP
builder.Services.AddHttpClient<NbpService>(client =>
{
    client.BaseAddress = new Uri("https://api.nbp.pl/api/");
});

// Rejestracja HttpClient dla DogService z bazowym adresem API Dog CEO
builder.Services.AddHttpClient<DogService>(client =>
{
    client.BaseAddress = new Uri("https://dog.ceo/api/");
});

// Rejestracja HttpClient dla JsonPlaceholderService (implementującego IJsonPlaceholderService)
builder.Services.AddHttpClient<IJsonPlaceholderService, JsonPlaceholderService>();

var app = builder.Build();

// Jeśli środowisko to Development, włączamy Swagger i UI do testowania API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Rejestracja middleware obsługującego globalnie błędy HTTP i zwracającego ustandaryzowane odpowiedzi
app.UseMiddleware<ErrorHandlingMiddleware>();

// Wymuszamy przekierowanie HTTP na HTTPS
app.UseHttpsRedirection();

// Ustawiamy obsługę plików statycznych i domyślnych (np. index.html)
app.UseDefaultFiles();
app.UseStaticFiles();

// Konfiguracja routingu HTTP
app.UseRouting();

// Włączamy mechanizmy uwierzytelniania i autoryzacji w pipeline żądań
app.UseAuthentication();
app.UseAuthorization();

// Mapowanie endpointów kontrolerów API
app.MapControllers();

// Uruchamiamy aplikację
app.Run();
