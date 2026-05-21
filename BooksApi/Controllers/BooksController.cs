using Microsoft.AspNetCore.Mvc;
using BooksApi.Models;

namespace BooksApi.Controllers
{

    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private static List<Book> _books = new()
    {
        new Book { Id = 1, Title = "Кобзар", Author = "Тарас Шевченко", Year = 1840 },
        new Book { Id = 2, Title = "Лісова пісня", Author = "Леся Українка", Year = 1911 },
        new Book { Id = 3, Title = "Захар Беркут", Author = "Іван Франко", Year = 1883 }
    };

        private static int _nextId = 4;

        // GET /api/books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(_books);
        }

        // GET /api/books/{id}
        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound($"Книгу з ID {id} не знайдено");

            return Ok(book);
        }

        // POST /api/books
        [HttpPost]
        public ActionResult<Book> Create([FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            book.Id = _nextId++;
            _books.Add(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // PUT /api/books/{id}
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Book updatedBook)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound($"Книгу з ID {id} не знайдено");

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Year = updatedBook.Year;

            return NoContent();
        }

        // DELETE /api/books/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound($"Книгу з ID {id} не знайдено");

            _books.Remove(book);
            return NoContent();
        }
    }
}