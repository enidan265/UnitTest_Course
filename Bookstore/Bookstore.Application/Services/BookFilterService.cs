using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Domain.Entities;

namespace Bookstore.Application.Services;

public class BookFilterService
{
    public IBookRepository BookRepository { get; }

    public BookFilterService(IBookRepository bookRepository)
    {
        BookRepository = bookRepository;
    }

    public async Task<List<Book>> GetFilteredBooksAsync(BookFilter bookFilter)
    {
        return await BookRepository.GetFilteredBooksAsync(bookFilter);
    }

}
