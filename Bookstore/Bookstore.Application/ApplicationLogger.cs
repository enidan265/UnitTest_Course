using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application;

public class ApplicationLogger<T> : IApplicationLogger<T>
{
    public ILogger<T> Logger { get; }

    public ApplicationLogger(ILogger<T> logger)
    {
        Logger = logger;
    }

    public void LogCreateAuthorAsyncCalled(AuthorCreate authorCreate)
    {
        Logger.LogInformation($"AuthorCreate called. {authorCreate}");
    }

    public void LogValidationErrorInCreateAuthor(ValidationException ex, AuthorCreate authorCreate)
    {
        Logger.LogError(ex, $"Validation Error in CreateAuthor. {authorCreate}");
    }

    public void LogAuthorCreated(long id)
    {
        Logger.LogInformation($"Author created. {id}");
    }

    public void LogAuthorNotFound(long authorId)
    {
        Logger.LogError($"Author not found in UpdateAuthor. {authorId}");
    }

    public void LogAuthorUpdated(Author author)
    {
        Logger.LogInformation($"Author updated. {author}");
    }

    public void LogUpdateAuthorAsyncCalled(AuthorUpdate authorUpdate)
    {
        Logger.LogInformation($"AuthorUpdate called. {authorUpdate}");
    }

    public void LogValidationErrorInUpdateAuthor(ValidationException ex, AuthorUpdate authorUpdate)
    {
        Logger.LogError(ex, $"Validation Error in UpdateAuthor. {authorUpdate}");
    }
}
