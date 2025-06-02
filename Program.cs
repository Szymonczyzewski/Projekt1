// Program.cs
using FinanceManagerApi.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Net.Http; // Dodaj to
using Projekt; // Dodaj to, aby NbpService był dostępny

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Dodaj DbContext do usług aplikacji
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(9, 3, 0)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    ));

// Dodaj usługi Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dodaj HttpClient i NbpService do kontenera DI
builder.Services.AddHttpClient<NbpService>(client =>
{
    client.BaseAddress = new Uri("http://api.nbp.pl/api/");
});

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