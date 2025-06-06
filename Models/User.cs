// Models/User.cs

using System.ComponentModel.DataAnnotations; // Biblioteka do atrybutów walidacyjnych

namespace FinanceManagerApi.Models
{
    // Klasa reprezentuje użytkownika w bazie danych
    public class User
    {
        [Key] 
        // Oznacza, że właściwość Id jest kluczem głównym tabeli w bazie danych
        public int Id { get; set; }

        [Required] 
        // Pole wymagane, nie może być puste
        [EmailAddress] 
        // Waliduje, czy wartość jest poprawnym adresem e-mail
        [MaxLength(255)] 
        // Maksymalna dozwolona długość tekstu to 255 znaków
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        // Hasło powinno być przechowywane jako hash, a nie w czystym tekście!
        public string PasswordHash { get; set; } = string.Empty;

        // Przykład pola do przechowywania roli użytkownika (opcjonalne)
        // public string Role { get; set; } = "User";
    }
}
