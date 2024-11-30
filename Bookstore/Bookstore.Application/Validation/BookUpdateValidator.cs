using Bookstore.Application.Dtos;
using FluentValidation;

namespace Bookstore.Application.Validation;

public class BookUpdateValidator : AbstractValidator<BookUpdate>
{
    public BookUpdateValidator()
    {
        RuleFor(bc => bc.Title).NotEmpty().MaximumLength(100);
    }
}
