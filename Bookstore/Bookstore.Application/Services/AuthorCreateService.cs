using AutoMapper;
using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using FluentValidation;

namespace Bookstore.Application.Services;

public class AuthorCreateService
{
    public IAuthorRepository AuthorRepository { get; }
    public IMapper Mapper { get; }
    public AuthorCreateValidator AuthorCreateValidator { get; }
    public IApplicationLogger<AuthorCreateService> Logger { get; }

    public AuthorCreateService(IAuthorRepository authorRepository, IMapper mapper,
        AuthorCreateValidator authorCreateValidator, IApplicationLogger<AuthorCreateService> logger)
    {
        AuthorRepository = authorRepository;
        Mapper = mapper;
        AuthorCreateValidator = authorCreateValidator;
        Logger = logger;
    }

    public async Task<long> CreateAuthorAsync(AuthorCreate authorCreate)
    {
        Logger.LogCreateAuthorAsyncCalled(authorCreate);
        try
        {
            await AuthorCreateValidator.ValidateAndThrowAsync(authorCreate);
        }
        catch(ValidationException ex)
        {
            Logger.LogValidationErrorInCreateAuthor(ex, authorCreate);
            throw;
        }
        var author = Mapper.Map<Author>(authorCreate);
        long id = await AuthorRepository.AddAuthorAsync(author);
        Logger.LogAuthorCreated(id); 
        return id;
    }

}
