using BooksApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace BookApi.Tests;

public class BooksApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BooksApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    //перевірка статусу 200 та непорожнього списку
    [Fact]
    public async Task GetAll_ReturnsOkAndNonEmptyList()
    {
        var response = await _client.GetAsync("/api/books");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var books = await response.Content.ReadFromJsonAsync<List<Book>>();
        Assert.NotNull(books);
        Assert.NotEmpty(books);
    }

    //існуючий ID повертає 200
    [Fact]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/books/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var book = await response.Content.ReadFromJsonAsync<Book>();
        Assert.NotNull(book);
        Assert.Equal(1, book.Id);
    }

    //неіснуючий ID повертає 404
    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/books/9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    //створення книги повертає 201
    [Fact]
    public async Task Create_ValidBook_ReturnsCreated()
    {
        var newBook = new Book { Title = "Нова книга", Author = "Автор", Year = 2024 };

        var response = await _client.PostAsJsonAsync("/api/books", newBook);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Book>();
        Assert.NotNull(created);
        Assert.Equal("Нова книга", created.Title);
    }

    //порожній Title повертає 400
    [Fact]
    public async Task Create_EmptyTitle_ReturnsBadRequest()
    {
        var invalidBook = new Book { Title = "", Author = "Автор", Year = 2024 };

        var response = await _client.PostAsJsonAsync("/api/books", invalidBook);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    //порожній Author повертає 400
    [Fact]
    public async Task Create_EmptyAuthor_ReturnsBadRequest()
    {
        var invalidBook = new Book { Title = "Назва", Author = "", Year = 2024 };

        var response = await _client.PostAsJsonAsync("/api/books", invalidBook);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    //оновлення існуючої книги повертає 204
    [Fact]
    public async Task Update_ExistingId_ReturnsNoContent()
    {
        var updatedBook = new Book { Title = "Оновлена книга", Author = "Оновлений автор", Year = 2020 };

        var response = await _client.PutAsJsonAsync("/api/books/1", updatedBook);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    //неіснуючий ID повертає 404
    [Fact]
    public async Task Update_NonExistingId_ReturnsNotFound()
    {
        var updatedBook = new Book { Title = "Книга", Author = "Автор", Year = 2020 };

        var response = await _client.PutAsJsonAsync("/api/books/9999", updatedBook);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    //видалення повертає 204
    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/api/books/2");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    //перевірка що книга більше недоступна
    [Fact]
    public async Task Delete_ExistingId_BookNoLongerAvailable()
    {
        await _client.DeleteAsync("/api/books/3");

        var response = await _client.GetAsync("/api/books/3");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}