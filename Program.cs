// Program.cs
using FinanceManagerApi.Data; // Twój DbContext
using Microsoft.EntityFrameworkCore; // Potrzebne dla UseMySql
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // Potrzebne dla MySqlServerVersion

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Dodaj DbContext do usług aplikacji
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(9, 3, 0)), // Dostosuj wersję MySQL!
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    ));

// Dodaj usługi Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
