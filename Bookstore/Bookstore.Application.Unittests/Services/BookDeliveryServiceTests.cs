using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Exceptions;
using Bookstore.Application.Services;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.Application.Unittests.Services
{
    public class BookDeliveryServiceTests
    {
        private BookDeliveryValidator BookDeliveryValidator { get; }

        public BookDeliveryServiceTests()
        {
            BookDeliveryValidator = new BookDeliveryValidator();
        }

        [Fact]
        public async Task Quantity_Increased()
        {
            //Arrange
            var bookDelivery = new BookDelivery(1, 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            var book = new Book();
            bookRepositoryMock.Setup(mock => mock.GetBookByIdAsync(1))
                .ReturnsAsync(book);
            var bookDeliveryService = new BookDeliveryService(bookRepositoryMock.Object,
                BookDeliveryValidator);

            //Act
            await bookDeliveryService.ProcessBookDeliveryAsync(bookDelivery);

            //Assert
            Assert.Equal(1, book.Quantity);
            bookRepositoryMock.Verify(mock => mock.UpdateAsync(), Times.Once);
        }

        [Fact]
        public void BookNotFoundException_For_Non_Existent_BookId()
        {
            //Arrange
            var bookDelivery = new BookDelivery(1, 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(mock => mock.GetBookByIdAsync(1))
                .Returns<Book?>(null);
            var bookDeliveryService = new BookDeliveryService(bookRepositoryMock.Object,
                BookDeliveryValidator);

            //Act
            Func<Task> func = async () => await
            bookDeliveryService.ProcessBookDeliveryAsync(bookDelivery);

            //Assert
            Assert.ThrowsAsync<BookNotFoundException>(func);
        }
    }
}
