using FluentValidation;
using AutoMapper;
using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Services;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;

namespace Bookstore.Application.Unittests.Services
{
    public class AuthorCreateServiceTests
    {
        private IMapper Mapper { get; }
        private AuthorCreateValidator Validator { get; }

        public AuthorCreateServiceTests()
        {
            Mapper = new MapperConfiguration(cfg =>
            cfg.AddMaps(typeof(DtoEntityMapperProfile))).CreateMapper();
            Validator = new AuthorCreateValidator();
        }

        [Fact]
        public async Task Author_Created()
        {
            //Arrange
            var authorCreate = new AuthorCreate("Test", "Test");
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock.Setup(authorRepositoryMock 
            => authorRepositoryMock.AddAuthorAsync(It.IsAny<Author>())).ReturnsAsync(1);
            var applicationLoggerMock = new Mock<IApplicationLogger<AuthorCreateService>>();
            var authorCreateSerice = new AuthorCreateService(authorRepositoryMock.Object,
                Mapper, Validator, applicationLoggerMock.Object);

            //Act
            await authorCreateSerice.CreateAuthorAsync(authorCreate);

            //Assert
            authorRepositoryMock.Verify(authorRepositoryMock =>
            authorRepositoryMock.AddAuthorAsync(It.IsAny<Author>()), Times.Once);
            applicationLoggerMock.Verify(applicationLoggerMock =>
            applicationLoggerMock.LogCreateAuthorAsyncCalled(authorCreate), Times.Once);
            applicationLoggerMock.Verify(applicationLoggerMock =>
            applicationLoggerMock.LogAuthorCreated(1), Times.Once);
        }


        [Fact]
        public async Task ValidationException_For_Invalid_AuthorCreate()
        {
            //Arrange
            var authorCreate = new AuthorCreate(string.Empty, "Test");
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var applicationLoggerMock = new Mock<IApplicationLogger<AuthorCreateService>>();
            var authorCreateSerice = new AuthorCreateService(authorRepositoryMock.Object,
                Mapper, Validator, applicationLoggerMock.Object);

            //Act
            try
            {
                await authorCreateSerice.CreateAuthorAsync(authorCreate);
                throw new Exception("Supposed to throw ValidationException");
            }
            catch(ValidationException ex)
            {
                //Assert
                applicationLoggerMock.Verify(applicationLoggerMock =>
                applicationLoggerMock.LogCreateAuthorAsyncCalled(authorCreate), Times.Once);
                applicationLoggerMock.Verify(applicationLoggerMock =>
                applicationLoggerMock.LogValidationErrorInCreateAuthor(ex, authorCreate), Times.Once);
            }
        }
    }
}
