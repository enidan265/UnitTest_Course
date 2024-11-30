using Bookstore.Application.Dtos;
using Bookstore.Application.Validation;
using Xunit;

namespace Bookstore.Application.Unittests.Validation;

public class AuthorUpdateValidatorTests
{
    private AuthorUpdateValidator AuthorUpdateValidator { get; }
     = new AuthorUpdateValidator();

    [Fact]
    public void Valid_AuthorUpdate_Passes_Validation()
    {
        //Arrange
        var authorUpdate = new AuthorUpdate(1, "Test", "Test");

        //Act
        var result = AuthorUpdateValidator.Validate(authorUpdate);

        //Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validation_Error_For_Empty_Firstname()
    {
        //Arrange
        var authorUpdate = new AuthorUpdate(1, string.Empty, "Test");

        //Act
        var result = AuthorUpdateValidator.Validate(authorUpdate);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("NotEmptyValidator")
        && error.PropertyName.Equals("Firstname"));
    }

    [Fact]
    public void Validation_Error_For_Empty_Lastname()
    {
        //Arrange
        var authorUpdate = new AuthorUpdate(1, "Test", string.Empty);

        //Act
        var result = AuthorUpdateValidator.Validate(authorUpdate);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("NotEmptyValidator")
        && error.PropertyName.Equals("Lastname"));
    }

    [Fact]
    public void Validation_Error_For_Too_Long_Firstname()
    {
        //Arrange
        var authorUpdate = new AuthorUpdate(1, @"AAAAAAAAAAAAAAAAAAAA
                            AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "Test");

        //Act
        var result = AuthorUpdateValidator.Validate(authorUpdate);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("MaximumLengthValidator")
        && error.PropertyName.Equals("Firstname"));
    }

    [Fact]
    public void Validation_Error_For_Too_Long_Lastname()
    {
        //Arrange
        var authorUpdate = new AuthorUpdate(1, "Test", @"AAAAAAAAAAAAAAAAAAAA
                            AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

        //Act
        var result = AuthorUpdateValidator.Validate(authorUpdate);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("MaximumLengthValidator")
        && error.PropertyName.Equals("Lastname"));
    }
}
