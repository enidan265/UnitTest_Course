using Bookstore.Application.Dtos;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.Integrationtests;

public class AuthorUpdateTests : IntegrationtestBase, IDisposable
{
    private Author Author { get; }

    public AuthorUpdateTests(WebApplicationFactory<Startup> factory) : base(factory)
    {
        Author = new Author()
        {
            Firstname = "Test",
            Lastname = "Test"
        };

        DbContext.Authors.Add(Author);
        DbContext.SaveChanges();
    }

    [Fact]
    public async Task Success_StatusCode_For_Updated_Author()
    {
        //Arrange
        var authorUpdate = new AuthorUpdate(Author.Id, "Test", "Test2");
        var authorUpdateJson = JsonConvert.SerializeObject(authorUpdate);
        var content = new StringContent(authorUpdateJson, Encoding.UTF8,
            "application/json");
        var expectedAuthor = Mapper.Map<Author>(authorUpdate);

        //Act
        var response = await Client.PutAsync("Author/Update", content);

        //Get Author from Db
        await DbContext.Entry(Author).ReloadAsync();


        //Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(expectedAuthor, Author);
    }

    [Fact]
    public async Task StatusCode_400_For_Non_Existent_Author()
    {
        //Arrange
        var authorUpdate = new AuthorUpdate(long.MaxValue, "Test", "Test2");
        var authorUpdateJson = JsonConvert.SerializeObject(authorUpdate);
        var content = new StringContent(authorUpdateJson, Encoding.UTF8,
            "application/json");

        //Act
        var response = await Client.PutAsync("Author/Update", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Author not found", responseContent);
    }

    [Fact]
    public async Task StatusCode_400_For_ValidationError()
    {
        //Arrange
        var authorUpdate = new AuthorUpdate(long.MaxValue, "Test", string.Empty);
        var authorUpdateJson = JsonConvert.SerializeObject(authorUpdate);
        var content = new StringContent(authorUpdateJson, Encoding.UTF8,
            "application/json");

        //Act
        var response = await Client.PutAsync("Author/Update", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Validation Error", responseContent);
    }

    public void Dispose()
    {
        DbContext.Authors.Remove(Author);
        DbContext.SaveChanges();
        DbContext.Dispose();
    }
}
