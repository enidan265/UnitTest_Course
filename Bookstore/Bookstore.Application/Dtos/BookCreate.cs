namespace Bookstore.Application.Dtos;

public record BookCreate(string Isbn, string Title, long AuthorId, int Quantity);
