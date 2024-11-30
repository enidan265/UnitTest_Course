using Bookstore.Application.Dtos;
using Bookstore.Application.Validation;
using Xunit;

namespace Bookstore.Application.Unittests.Validation;

public class AuthorCreateValidatorTests
{
    private AuthorCreateValidator AuthorCreateValidator { get; }
     = new AuthorCreateValidator();

    [Fact]
    public void Valid_AuthorCreate_Passes_Validation()
    {
        //Arrange
        var authorCreate = new AuthorCreate("Test", "Test");

        //Act
        var result = AuthorCreateValidator.Validate(authorCreate);

        //Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validation_Error_For_Empty_Firstname()
    {
        //Arrange
        var authorCreate = new AuthorCreate(string.Empty, "Test");

        //Act
        var result = AuthorCreateValidator.Validate(authorCreate);

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
        var authorCreate = new AuthorCreate("Test", string.Empty);

        //Act
        var result = AuthorCreateValidator.Validate(authorCreate);

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
        var authorCreate = new AuthorCreate(@"AAAAAAAAAAAAAAAAAAAA
                            AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "Test");

        //Act
        var result = AuthorCreateValidator.Validate(authorCreate);

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
        var authorCreate = new AuthorCreate("Test", @"AAAAAAAAAAAAAAAAAAAA
                            AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

        //Act
        var result = AuthorCreateValidator.Validate(authorCreate);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, error =>
        error.ErrorCode.Equals("MaximumLengthValidator")
        && error.PropertyName.Equals("Lastname"));
    }
}
