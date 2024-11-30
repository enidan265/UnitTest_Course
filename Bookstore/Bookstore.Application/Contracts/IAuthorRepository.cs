using Bookstore.Domain.Entities;

namespace Bookstore.Application.Contracts;

public interface IAuthorRepository
{
    Task<long> AddAuthorAsync(Author author);
    Task<Author?> GetAuthorByIdAsync(long authorId);
    Task UpdateAsync();
}
