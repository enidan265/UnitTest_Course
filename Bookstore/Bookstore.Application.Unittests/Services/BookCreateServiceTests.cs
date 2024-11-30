using AutoMapper;
using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Exceptions;
using Bookstore.Application.Services;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Validation;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.Application.Unittests.Services;

public class BookCreateServiceTests
{
    private IMapper Mapper { get; }
    private BookCreateValidator BookCreateValidator { get; }
    private BookValidator BookValidator { get; }

    public BookCreateServiceTests()
    {
        Mapper = new MapperConfiguration(cfg =>
        cfg.AddMaps(typeof(DtoEntityMapperProfile))).CreateMapper();
        BookValidator = new BookValidator();
        BookCreateValidator = new BookCreateValidator();
    }

    [Fact]
    public async Task Book_Added()
    {
        //Arrange
        var bookCreate = new BookCreate("1234567891234", "Test", 1, 0);
        var bookRepositoryMock = new Mock<IBookRepository>();
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        authorRepositoryMock.Setup(mock => mock.GetAuthorByIdAsync(1))
            .ReturnsAsync(new Author());
        var bookCreateService = new BookCreateService(bookRepositoryMock.Object,
            authorRepositoryMock.Object, Mapper, BookCreateValidator,
            BookValidator);

        //Act
        await bookCreateService.CreateBookAsync(bookCreate);

        //Assert
        bookRepositoryMock.Verify(mock => mock.AddBookAsync(It.IsAny<Book>()),
            Times.Once);
    }

    [Fact]
    public void AuthorNotFoundException_For_Non_Existent_Author()
    {
        //Arrange
        var bookCreate = new BookCreate("1234567891234", "Test", 1, 0);
        var bookRepositoryMock = new Mock<IBookRepository>();
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        authorRepositoryMock.Setup(mock => mock.GetAuthorByIdAsync(1))
            .Returns<Author?>(null);
        var bookCreateService = new BookCreateService(bookRepositoryMock.Object,
            authorRepositoryMock.Object, Mapper, BookCreateValidator,
            BookValidator);

        //Act
        Func<Task> func = async () => await bookCreateService
        .CreateBookAsync(bookCreate);

        //Assert
        Assert.ThrowsAsync<AuthorNotFoundException>(func);
    }

    [Fact]
    public void IsbnDuplicateException_For_Existing_Isbn()
    {
        //Arrange
        var bookCreate = new BookCreate("1234567891234", "Test", 1, 0);
        var bookRepositoryMock = new Mock<IBookRepository>();
        bookRepositoryMock.Setup(mock => mock.GetBookByIsbnAsync("1234567891234"))
            .ReturnsAsync(new Book());
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        authorRepositoryMock.Setup(mock => mock.GetAuthorByIdAsync(1))
            .ReturnsAsync(new Author());
        var bookCreateService = new BookCreateService(bookRepositoryMock.Object,
            authorRepositoryMock.Object, Mapper, BookCreateValidator,
            BookValidator);

        //Act
        Func<Task> func = async () => await bookCreateService
        .CreateBookAsync(bookCreate);

        //Assert
        Assert.ThrowsAsync<IsbnDuplicateException>(func);
    }
}
