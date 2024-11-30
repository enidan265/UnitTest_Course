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

namespace Bookstore.Application.Unittests.Services
{
    public class BookSaleServiceTests
    {
        private BookSaleValidator BookSaleValidator { get; }
        private BookValidator BookValidator { get; }

        public BookSaleServiceTests()
        {
            BookSaleValidator = new BookSaleValidator();
            BookValidator = new BookValidator();
        }

        [Fact]
        public async Task Quantity_Decreased()
        {
            //Arrange
            var bookSale = new BookSale(1, 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            var book = new Book() { Quantity = 1, Author = new Author() };
            bookRepositoryMock.Setup(mock => mock.GetBookByIdAsync(1))
                .ReturnsAsync(book);
            var bookSaleService = new BookSaleService(bookRepositoryMock.Object,
                BookSaleValidator, BookValidator);

            //Act
            await bookSaleService.ProcessBookSaleAsync(bookSale);

            //Assert
            Assert.Equal(0, book.Quantity);
            bookRepositoryMock.Verify(mock => mock.UpdateAsync(), Times.Once);
        }

        [Fact]
        public void BookNotFoundException_For_Non_Existent_BookId()
        {
            //Arrange
            var bookSale = new BookSale(1, 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(mock => mock.GetBookByIdAsync(1))
                .Returns<Book?>(null);
            var bookSaleService = new BookSaleService(bookRepositoryMock.Object,
                BookSaleValidator, BookValidator);

            //Act
            Func<Task> func = async () => await
            bookSaleService.ProcessBookSaleAsync(bookSale);

            //Assert
            Assert.ThrowsAsync<BookNotFoundException>(func);
        }
    }
}
