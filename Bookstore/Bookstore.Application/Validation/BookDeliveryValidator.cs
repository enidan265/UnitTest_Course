using Bookstore.Application.Dtos;
using FluentValidation;

namespace Bookstore.Application.Validation;

public class BookDeliveryValidator : AbstractValidator<BookDelivery>
{
    public BookDeliveryValidator()
    {
        RuleFor(delivery => delivery.Quantity).GreaterThan(0);
    }
}
