// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using FinanceManagerApi.Models; // Będziemy tworzyć ten model


namespace FinanceManagerApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Dodaj DbSet dla każdego modelu, który chcesz przechowywać w bazie danych
        public DbSet<User> Users { get; set; }
        // public DbSet<Transaction> Transactions { get; set; } // Dodamy później
        // public DbSet<Category> Categories { get; set; } // Dodamy później

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tutaj możesz skonfigurować dodatkowe relacje, indeksy, itp.
            // Na przykład, aby zapewnić unikalność adresu e-mail użytkownika:
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}



