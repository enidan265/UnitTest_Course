using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        public ApplicationDbContext DbContext { get; }

        public BookRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<long> AddBookAsync(Book book)
        {
            await DbContext.Books.AddAsync(book);
            await DbContext.SaveChangesAsync();
            return book.Id;
        }

        public async Task<Book?> GetBookByIdAsync(long id)
        {
            return await DbContext.Books.Include(book => book.Author).Where(book => book.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<Book?> GetBookByIsbnAsync(string isbn)
        {
            return await DbContext.Books.Where(book => book.Isbn == isbn)
                .SingleOrDefaultAsync();
        }

        public async Task<List<Book>> GetFilteredBooksAsync(BookFilter bookFilter)
        {
            if (string.IsNullOrEmpty(bookFilter.SearchTerm))
                return await DbContext.Books.ToListAsync();

            return await DbContext.Books.Where(book => book.Isbn.Contains(bookFilter.SearchTerm)
            || book.Title.Contains(bookFilter.SearchTerm)).ToListAsync();
        }

        public async Task UpdateAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
