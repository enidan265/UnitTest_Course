using AutoMapper;
using Bookstore.Application;
using Bookstore.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Xunit;

namespace Bookstore.Integrationtests;

public class IntegrationtestBase : IClassFixture<WebApplicationFactory<Startup>>
{
    protected WebApplicationFactory<Startup> Factory { get; }
    protected HttpClient Client { get; }
    protected ApplicationDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    public IntegrationtestBase(WebApplicationFactory<Startup> factory)
    {
        Environment.SetEnvironmentVariable("ADMIN_EMAIL", "admin@test.de");
        Environment.SetEnvironmentVariable("ADMIN_PW", "Admin!123Admin?");
        Factory = factory;
        Client = factory.CreateClient();
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>() ??
            throw new Exception("Scope factory not found");
        var scope = scopeFactory.CreateScope() ??
            throw new Exception("Could not create Scope");
        DbContext = scope.ServiceProvider.GetService<ApplicationDbContext>() ??
            throw new Exception("Could not get ApplicationDbContext");
        Mapper = new MapperConfiguration(cfg =>
        cfg.AddMaps(typeof(DtoEntityMapperProfile))).CreateMapper();
    }

}
