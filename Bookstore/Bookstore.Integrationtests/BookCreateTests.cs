using Bookstore.Application.Dtos;
using Bookstore.Domain.Entities;
using Bookstore.Integrationtests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.Integrationtests
{
    [TestCaseOrderer("Bookstore.Integrationtests.Utils.PriotizedOrderer", "Bookstore.Integrationtests")]
    public class BookCreateTests : IntegrationtestBase, IDisposable, IClassFixture<AuthorFixture>
    {
        public AuthorFixture AuthorFixture { get; }

        public BookCreateTests(WebApplicationFactory<Startup> factory, AuthorFixture authorFixture) : base(factory)
        {
            AuthorFixture = authorFixture;
        }

        [Fact, TestPriority(0)]
        public async Task Success_StatusCode_For_Created_Book()
        {
            //Arrange
            DbContext.Authors.Add(AuthorFixture.Author);
            await DbContext.SaveChangesAsync();

            var bookCreate = new BookCreate("1234567891234", "Test", AuthorFixture.Author.Id, 1);
            var bookCreateJson = JsonConvert.SerializeObject(bookCreate);
            var content = new StringContent(bookCreateJson, Encoding.UTF8,
                "application/json");
            var expectedBook = Mapper.Map<Book>(bookCreate);

            //Act
            var response = await Client.PostAsync("/Book/Create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Get Book from Db
            var bookInDb = await DbContext.Books.Where(book => book.Isbn.Equals(bookCreate.Isbn))
                .SingleAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectedBook, bookInDb);
        }

        [Fact, TestPriority(1)]
        public async Task StatusCode_400_For_Non_Existent_Author()
        {
            //Arrange
            var bookCreate = new BookCreate("1234567891235", "Test", int.MaxValue, 1);
            var bookCreateJson = JsonConvert.SerializeObject(bookCreate);
            var content = new StringContent(bookCreateJson, Encoding.UTF8,
                "application/json");
            var expectedBook = Mapper.Map<Book>(bookCreate);

            //Act
            var response = await Client.PostAsync("/Book/Create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Contains("Author not found", responseContent);
        }

        [Fact, TestPriority(2)]
        public async Task StatusCode_400_For_Duplicate_Isbn()
        {
            //Arrange
            var bookCreate = new BookCreate("1234567891234", "Test", AuthorFixture.Author.Id, 1);
            var bookCreateJson = JsonConvert.SerializeObject(bookCreate);
            var content = new StringContent(bookCreateJson, Encoding.UTF8,
                "application/json");
            var expectedBook = Mapper.Map<Book>(bookCreate);

            //Act
            var response = await Client.PostAsync("/Book/Create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Contains("Isbn already Exists", responseContent);
        }

        [Fact, TestPriority(3)]
        public async Task StatusCode_400_For_ValidationError()
        {
            //Arrange
            var bookCreate = new BookCreate("123456", "Test", AuthorFixture.Author.Id, 1);
            var bookCreateJson = JsonConvert.SerializeObject(bookCreate);
            var content = new StringContent(bookCreateJson, Encoding.UTF8,
                "application/json");
            var expectedBook = Mapper.Map<Book>(bookCreate);

            //Act
            var response = await Client.PostAsync("/Book/Create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Contains("Validation Error", responseContent);

            //Teardown
            DbContext.Authors.Remove(AuthorFixture.Author);
            await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
