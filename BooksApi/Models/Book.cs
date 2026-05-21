using System.ComponentModel.DataAnnotations;

namespace BooksApi.Models
{

    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва є обов'язковою")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Автор є обов'язковим")]
        public string Author { get; set; } = string.Empty;

        public int Year { get; set; }
    }
}