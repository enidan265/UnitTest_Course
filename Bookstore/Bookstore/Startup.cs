using Bookstore.Application;
using Bookstore.Application.Contracts;
using Bookstore.Infrastructure;
using Bookstore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        DIConfiguration.RegisterServices(services);
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped(typeof(IApplicationLogger<>), typeof(ApplicationLogger<>));
        services.AddDbContext<ApplicationDbContext>();

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "auth_cookie";
            options.Cookie.SameSite = SameSiteMode.None;

            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });

        services.AddScoped<IdentitySeed>();
    }

    public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime,
        ApplicationDbContext context, IdentitySeed identitySeed)
    {
        context.Database.EnsureCreated();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
        });
        identitySeed.Seed().Wait();
    }
}