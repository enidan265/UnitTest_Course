using Bookstore.Domain.Entities;
using Bookstore.Domain.Validation;
using Xunit;

namespace Bookstore.Domain.Unittests.Validation
{
    public class BookValidatorTests
    {
        private Author Author { get; } =
            new()
            {
                Id = 1,
                Firstname = "Test",
                Lastname = "Test"
            };

        private BookValidator BookValidator { get; } = new();

        [Fact]
        public void Valid_Book_Passes_Validation()
        {
            //Arrange
            var book = new Book()
            {
                Id = 1,
                Author = Author,
                Isbn = "1234567891234",
                Title = "Test"
            };

            //Act
            var result = BookValidator.Validate(book);

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validation_Error_For_Missing_Author()
        {
            //Arrange
            var book = new Book()
            {
                Id = 1,
                Isbn = "1234567891234",
                Title = "Test"
            };

            //Act
            var result = BookValidator.Validate(book);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Contains(result.Errors, error =>
            error.ErrorCode.Equals("NotNullValidator") &&
            error.PropertyName.Equals("Author"));
        }

        [Fact]
        public void Validation_Error_For_Negative_Quantity()
        {
            //Arrange
            var book = new Book()
            {
                Id = 1,
                Author = Author,
                Quantity = -1,
                Isbn = "1234567891234",
                Title = "Test"
            };

            //Act
            var result = BookValidator.Validate(book);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Contains(result.Errors, error =>
            error.ErrorCode.Equals("GreaterThanOrEqualValidator") &&
            error.PropertyName.Equals("Quantity"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("")]
        [InlineData("123456789123")]
        [InlineData("12345678912345")]
        public void Validation_Error_For_Isbn_Of_Wrong_Length(string isbn)
        {
            //Arrange
            var book = new Book()
            {
                Id = 1,
                Author = Author,
                Quantity = 0,
                Isbn = isbn,
                Title = "Test"
            };

            //Act
            var result = BookValidator.Validate(book);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Contains(result.Errors, error =>
            error.ErrorCode.Equals("ExactLengthValidator") &&
            error.PropertyName.Equals("Isbn"));
        }
    }
}