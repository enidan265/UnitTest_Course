using AutoMapper;
using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Exceptions;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Validation;
using FluentValidation;

namespace Bookstore.Application.Services
{
    public class BookCreateService
    {
        public IBookRepository BookRepository { get; }
        public IAuthorRepository AuthorRepository { get; }
        public IMapper Mapper { get; }
        public BookCreateValidator BookCreateValidator { get; }
        public BookValidator BookValidator { get; }

        public BookCreateService(IBookRepository bookRepository,
            IAuthorRepository authorRepository, IMapper mapper,
            BookCreateValidator bookCreateValidator,
            BookValidator bookValidator)
        {
            BookRepository = bookRepository;
            AuthorRepository = authorRepository;
            Mapper = mapper;
            BookCreateValidator = bookCreateValidator;
            BookValidator = bookValidator;
        }

        public async Task<long> CreateBookAsync(BookCreate bookCreate)
        {
            await BookCreateValidator.ValidateAndThrowAsync(bookCreate);
            var book = Mapper.Map<Book>(bookCreate);
            Author? author = await AuthorRepository.GetAuthorByIdAsync(bookCreate.AuthorId);
            Book? existingBookForIsbn = await BookRepository.GetBookByIsbnAsync(bookCreate.Isbn);

            if (author == null)
                throw new AuthorNotFoundException();

            if (existingBookForIsbn != null)
                throw new IsbnDuplicateException();

            book.Author = author;
            await BookValidator.ValidateAndThrowAsync(book);

            var id = await BookRepository.AddBookAsync(book);
            return id;
        }

    }
}
