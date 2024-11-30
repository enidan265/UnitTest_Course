namespace Bookstore.Application.Dtos;

public record BookUpdate(long BookId, string Isbn, string Title, long AuthorId);
