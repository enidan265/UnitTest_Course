using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Book> Books { get; set; } = default!;
        public DbSet<Author> Authors { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Bookstore.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>().HasKey(e => e.Id);
            builder.Entity<Book>().HasOne(e => e.Author)
                .WithMany(e => e.Books)
                .HasForeignKey(e => e.AuthorId);
            builder.Entity<Book>().Property(e => e.Title).HasMaxLength(100);

            builder.Entity<Author>().HasKey(e => e.Id);
            builder.Entity<Author>().Property(e => e.Firstname).HasMaxLength(50);
            builder.Entity<Author>().Property(e => e.Lastname).HasMaxLength(50);
            base.OnModelCreating(builder);
        }
    }
}