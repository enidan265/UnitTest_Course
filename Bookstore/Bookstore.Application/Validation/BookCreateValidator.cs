using Bookstore.Application.Dtos;
using FluentValidation;

namespace Bookstore.Application.Validation;

public class BookCreateValidator : AbstractValidator<BookCreate>
{
    public BookCreateValidator()
    {
        RuleFor(bc => bc.Title).NotEmpty().MaximumLength(100);
    }
}
