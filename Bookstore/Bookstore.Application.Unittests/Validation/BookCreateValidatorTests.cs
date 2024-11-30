using Bookstore.Application.Dtos;
using Bookstore.Application.Validation;
using Xunit;

namespace Bookstore.Application.Unittests.Validation;

public class BookCreateValidatorTests
{
    private BookCreateValidator BookCreateValidator { get; }
     = new BookCreateValidator();

    [Fact]
    public void Valid_BookCreate_Passes_Validation()
    {
        //Arrange
        var bookCreate = new BookCreate("1234567891234", "Test", 1, 0);

        //Act
        var result = BookCreateValidator.Validate(bookCreate);

        //Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validation_Error_For_Empty_Title()
    {
        //Arrange
        var bookCreate = new BookCreate("1234567891234", string.Empty, 1, 0);

        //Act
        var result = BookCreateValidator.Validate(bookCreate);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("NotEmptyValidator")
        && error.PropertyName.Equals("Title"));
    }

    [Fact]
    public void Validation_Error_For_Too_Long_Title()
    {
        //Arrange
        var bookCreate = new BookCreate("1234567891234", @"AAAAAAAAAA
            AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            AAAAAAAAAA", 1, 0);

        //Act
        var result = BookCreateValidator.Validate(bookCreate);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("MaximumLengthValidator")
        && error.PropertyName.Equals("Title"));
    }
}
