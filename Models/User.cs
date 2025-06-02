// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace FinanceManagerApi.Models
{
    public class User
    {
        [Key] // Oznacza Id jako klucz główny
        public int Id { get; set; }

        [Required] // Wymagane pole
        [EmailAddress] // Walidacja formatu e-mail
        [MaxLength(255)] // Maksymalna długość
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        // Hasło powinno być przechowywane jako hash (nie jako czysty tekst!)
        public string PasswordHash { get; set; } = string.Empty;

        // Opcjonalnie, jeśli będziesz miał role użytkowników
        // public string Role { get; set; } = "User";
    }
}