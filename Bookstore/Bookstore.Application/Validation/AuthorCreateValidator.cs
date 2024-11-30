using Bookstore.Application.Dtos;
using FluentValidation;

namespace Bookstore.Application.Validation;

public class AuthorCreateValidator : AbstractValidator<AuthorCreate>
{
    public AuthorCreateValidator()
    {
        RuleFor(author => author.Firstname).NotEmpty().MaximumLength(50);
        RuleFor(author => author.Lastname).NotEmpty().MaximumLength(50);
    }
}
