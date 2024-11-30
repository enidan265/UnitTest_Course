using Bookstore.Application.Dtos;
using Bookstore.Application.Validation;
using Xunit;

namespace Bookstore.Application.Unittests.Validation;

public class BookDeliveryValidatorTests
{
    private BookDeliveryValidator BookDeliveryValidator { get; }
     = new BookDeliveryValidator();

    [Fact]
    public void Valid_BookDelivery_Passes_Validation()
    {
        //Arrange
        var bookDelivery = new BookDelivery(1, 1);

        //Act
        var result = BookDeliveryValidator.Validate(bookDelivery);

        //Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validation_Error_For_Quantity(int quantity)
    {
        //Arrange
        var bookDelivery = new BookDelivery(1, quantity);

        //Act
        var result = BookDeliveryValidator.Validate(bookDelivery);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("GreaterThanValidator")
        && error.PropertyName.Equals("Quantity"));
    }
}
