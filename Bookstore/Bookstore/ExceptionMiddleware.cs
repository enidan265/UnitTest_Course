using Bookstore.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ExceptionMiddleware
{
    private RequestDelegate Next { get; }

    public ExceptionMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (IsbnDuplicateException)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = string.Empty,
                Instance = "",
                Title = "Isbn already Exists.",
                Type = ""
            };

            var problemDetailsJson = JsonConvert.SerializeObject(problemDetails);
            await context.Response.WriteAsync(problemDetailsJson);
        }
        catch (AuthorNotFoundException)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = string.Empty,
                Instance = "",
                Title = "Author not found.",
                Type = ""
            };

            var problemDetailsJson = JsonConvert.SerializeObject(problemDetails);
            await context.Response.WriteAsync(problemDetailsJson);
        }
        catch (BookNotFoundException)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = string.Empty,
                Instance = "",
                Title = "Book not found.",
                Type = ""
            };

            var problemDetailsJson = JsonConvert.SerializeObject(problemDetails);
            await context.Response.WriteAsync(problemDetailsJson);
        }
        catch (ValidationException ex)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = JsonConvert.SerializeObject(ex.Errors),
                Instance = "",
                Title = "Validation Error",
                Type = ""
            };

            var problemDetailsJson = JsonConvert.SerializeObject(problemDetails);
            await context.Response.WriteAsync(problemDetailsJson);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                Instance = "",
                Title = "Internal Server Error - something went wrong",
                Type = ""
            };

            var problemDetailsJson = JsonConvert.SerializeObject(problemDetails);
            await context.Response.WriteAsync(problemDetailsJson);
        }
    }

}