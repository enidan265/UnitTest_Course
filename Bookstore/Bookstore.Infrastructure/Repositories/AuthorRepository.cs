using Bookstore.Application.Contracts;
using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        public ApplicationDbContext DbContext { get; }

        public AuthorRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<long> AddAuthorAsync(Author author)
        {
            await DbContext.Authors.AddAsync(author);
            await DbContext.SaveChangesAsync();
            return author.Id;
        }

        public async Task<Author?> GetAuthorByIdAsync(long authorId)
        {
            return await DbContext.Authors.Where(author => author.Id == authorId)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
