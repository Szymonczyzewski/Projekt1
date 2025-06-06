// Przestrzeń nazw projektu – porządkuje klasy w logiczne grupy
namespace Projekt.Models
{
    // Klasa reprezentuje pojedynczy post (np. z API JSONPlaceholder)
    public class Post
    {
        // Identyfikator użytkownika, który utworzył post
        public int UserId { get; set; }

        // Unikalny identyfikator posta
        public int Id { get; set; }

        // Tytuł posta, inicjalizowany jako pusty ciąg znaków, by uniknąć null
        public string Title { get; set; } = string.Empty;

        // Treść posta, również inicjalizowana jako pusty ciąg znaków
        public string Body { get; set; } = string.Empty;
    }
}
