// Importujemy bibliotekę Entity Framework Core – służy do pracy z bazą danych
using Microsoft.EntityFrameworkCore;

// Importujemy modele, które będą mapowane na tabele w bazie danych
using FinanceManagerApi.Models;

namespace FinanceManagerApi.Data
{
    // Klasa ApplicationDbContext reprezentuje "kontekst bazy danych"
    // Dzięki niej aplikacja wie, jak łączyć się z bazą i jakie tabele tam istnieją
    public class ApplicationDbContext : DbContext
    {
        // Konstruktor – przekazuje opcje (np. dane logowania do bazy) do klasy bazowej
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet to "reprezentacja tabeli" w bazie danych
        // Dzięki temu możemy odwoływać się do Users jak do tabeli i wykonywać operacje np. .Add(), .Find(), .ToList()
        public DbSet<User> Users { get; set; }

        // Można dodać więcej DbSetów w przyszłości, np.:
        // public DbSet<Transaction> Transactions { get; set; }
        // public DbSet<Category> Categories { get; set; }

        // Metoda wykonywana automatycznie przy tworzeniu modelu bazy danych
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Zawsze warto wywołać bazową implementację
            base.OnModelCreating(modelBuilder);

            // Przykład: konfigurujemy, aby adres e-mail użytkownika był unikalny
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)  // Dodajemy indeks do kolumny Email
                .IsUnique();             // Wymuszamy unikalność (czyli nie może być dwóch takich samych e-maili)
        }
    }
}
