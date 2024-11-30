using Bookstore.Application.Dtos;
using FluentValidation;

namespace Bookstore.Application.Validation;

public class BookSaleValidator : AbstractValidator<BookSale>
{
    public BookSaleValidator()
    {
        RuleFor(sale => sale.Quantity).GreaterThan(0);
    }
}
