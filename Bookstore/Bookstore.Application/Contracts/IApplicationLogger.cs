
using Bookstore.Application.Dtos;
using Bookstore.Domain.Entities;
using FluentValidation;

namespace Bookstore.Application.Contracts;

public interface IApplicationLogger<T>
{
    void LogAuthorCreated(long id);
    void LogAuthorNotFound(long authorId);
    void LogAuthorUpdated(Author author);
    void LogCreateAuthorAsyncCalled(AuthorCreate authorCreate);
    void LogUpdateAuthorAsyncCalled(AuthorUpdate authorUpdate);
    void LogValidationErrorInCreateAuthor(ValidationException ex, AuthorCreate authorCreate);
    void LogValidationErrorInUpdateAuthor(ValidationException ex, AuthorUpdate authorUpdate);
}
