using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Exceptions;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using FluentValidation;

namespace Bookstore.Application.Services;

public class BookDeliveryService
{
    public IBookRepository BookRepository { get; }
    public BookDeliveryValidator BookDeliveryValidator { get; }

    public BookDeliveryService(IBookRepository bookRepository,
        BookDeliveryValidator BookDeliveryValidator)
    {
        BookRepository = bookRepository;
        this.BookDeliveryValidator = BookDeliveryValidator;
    }

    public async Task ProcessBookDeliveryAsync(BookDelivery bookDelivery)
    {
        await BookDeliveryValidator.ValidateAndThrowAsync(bookDelivery);
        Book? book = await BookRepository.GetBookByIdAsync(bookDelivery.BookId);

        if (book == null)
            throw new BookNotFoundException();

        book.Quantity += bookDelivery.Quantity;

        await BookRepository.UpdateAsync();
    }

}
