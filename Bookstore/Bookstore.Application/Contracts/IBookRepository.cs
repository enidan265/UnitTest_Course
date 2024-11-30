using Bookstore.Application.Dtos;
using Bookstore.Domain.Entities;

namespace Bookstore.Application.Contracts;

public interface IBookRepository
{
    Task<Book?> GetBookByIsbnAsync(string isbn);
    Task<long> AddBookAsync(Book book);
    Task<Book?> GetBookByIdAsync(long id);
    Task UpdateAsync();
    Task<List<Book>> GetFilteredBooksAsync(BookFilter bookFilter);
}
