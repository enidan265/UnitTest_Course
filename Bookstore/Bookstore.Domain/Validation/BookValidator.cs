using Bookstore.Domain.Entities;
using FluentValidation;

namespace Bookstore.Domain.Validation;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.Isbn).Length(13);
        RuleFor(book => book.Quantity).GreaterThanOrEqualTo(0);
        RuleFor(book => book.Author).NotNull();
    }
}
