using Bookstore.Application.Dtos;
using Bookstore.Application.Validation;
using Xunit;

namespace Bookstore.Application.Unittests.Validation;

public class BookSaleValidatorTests
{
    private BookSaleValidator BookSaleValidator { get; }
     = new BookSaleValidator();

    [Fact]
    public void Valid_BookSale_Passes_Validation()
    {
        //Arrange
        var bookSale = new BookSale(1, 1);

        //Act
        var result = BookSaleValidator.Validate(bookSale);

        //Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validation_Error_For_Quantity(int quantity)
    {
        //Arrange
        var bookSale = new BookSale(1, quantity);

        //Act
        var result = BookSaleValidator.Validate(bookSale);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("GreaterThanValidator")
        && error.PropertyName.Equals("Quantity"));
    }
}
